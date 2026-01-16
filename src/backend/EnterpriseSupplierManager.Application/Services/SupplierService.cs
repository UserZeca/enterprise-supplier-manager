using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Exceptions;
using EnterpriseSupplierManager.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EnterpriseSupplierManager.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ICepService _cepService;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(
        ISupplierRepository supplierRepository, 
        ICompanyRepository companyRepository,
        ILogger<SupplierService> logger,
        ICepService cepService)
    {
        _supplierRepository = supplierRepository;
        _companyRepository = companyRepository;
        _logger = logger;
        _cepService = cepService;
    }

    public async Task<SupplierResponseDTO> CreateAsync(SupplierRequestDTO request)
    {
        var existing = await _supplierRepository.GetByDocumentAsync(request.Document);
        if (existing != null)
            throw new DuplicateEntryException("Este fornecedor já está cadastrado globalmente no sistema.");

        await _cepService.EnsureValidCepAsync(request.Cep);

        var supplier = request.Adapt<Supplier>();
        await _supplierRepository.AddAsync(supplier);

        return supplier.Adapt<SupplierResponseDTO>();
    }

    public async Task UpdateAsync(Guid id, SupplierRequestDTO request)
    {

        var supplier = await _supplierRepository.GetByIdWithCompaniesAsync(id)
            ?? throw new KeyNotFoundException("Fornecedor não encontrado.");

        _logger.LogInformation("Validando atualização para o fornecedor {SupplierId}", id);

        // Se for Pessoa Física, verifica o vínculo com o Paraná
        if (IsPhysicalPerson(request.Document))
        {
            bool linkedToParana = supplier.Companies.Any(c => c.Uf.ToUpper() == "PR");

            if (linkedToParana)
            {
                _logger.LogInformation("Fornecedor vinculado a empresa do PR. Validando idade.");
                ValidateParanaRule(request, "PR");
            }
        }

        if (supplier.Cep != request.Cep)
            await _cepService.EnsureValidCepAsync(request.Cep);

        request.Adapt(supplier);
        await _supplierRepository.UpdateAsync(supplier);

        _logger.LogInformation("Fornecedor {SupplierId} atualizado com sucesso.", id);
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Solicitação de exclusão para o fornecedor {SupplierId}", id);
        await _supplierRepository.DeleteAsync(id);
    }

    public async Task<SupplierResponseDTO?> GetByIdAsync(Guid id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        return supplier?.Adapt<SupplierResponseDTO>();
    }

    public async Task<IEnumerable<SupplierResponseDTO>> GetAllByCompanyIdAsync(Guid companyId)
    {
        var suppliers = await _supplierRepository.GetByCompanyIdAsync(companyId);
        return suppliers.Adapt<IEnumerable<SupplierResponseDTO>>();
    }

    public async Task AssociateToCompanyAsync(Guid supplierId, Guid companyId)
    {

        var supplier = await _supplierRepository.GetByIdAsync(supplierId)
            ?? throw new KeyNotFoundException("Fornecedor não encontrado.");

        var company = await _companyRepository.GetByIdAsync(companyId)
            ?? throw new KeyNotFoundException("Empresa não encontrada.");

        _logger.LogInformation("Iniciando governança para vínculo: Fornecedor {S} + Empresa {C}", supplierId, companyId);

        var supplierDto = supplier.Adapt<SupplierRequestDTO>();

        ValidateParanaRule(supplierDto, company.Uf);

        await _supplierRepository.AssociateToCompanyAsync(supplierId, companyId);

        _logger.LogInformation("Vínculo estabelecido com sucesso para a UF: {Uf}", company.Uf);
    }

    #region Métodos privados de validação

    private void ValidateParanaRule(SupplierRequestDTO supplier, string companyUf)
    {
        if (companyUf.ToUpper() == "PR" && IsPhysicalPerson(supplier.Document))
        {
            // Note que agora usamos os dados da entidade 'supplier' já cadastrada
            if (!supplier.BirthDate.HasValue)
                throw new ArgumentException("Fornecedor PF sem data de nascimento não pode ser vinculado a empresas do PR.");

            var age = CalculateAge(supplier.BirthDate.Value);
            if (age < 18)
                throw new ArgumentException($"O fornecedor {supplier.Name} é menor de idade ({age} anos) e não pode atender empresas no Paraná.");
        }
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

    private bool IsPhysicalPerson(string document) => document.Length == 11;

    #endregion
}