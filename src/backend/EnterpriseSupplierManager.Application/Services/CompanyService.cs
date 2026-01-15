using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Exceptions;
using EnterpriseSupplierManager.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EnterpriseSupplierManager.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICepService _cepService;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(
        ICompanyRepository companyRepository,
        ICepService cepService,
        ILogger<CompanyService> logger)
    {
        _companyRepository = companyRepository;
        _cepService = cepService;
        _logger = logger;
    }

    public async Task<IEnumerable<CompanyResponseDTO>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        return companies.Adapt<IEnumerable<CompanyResponseDTO>>();
    }

    public async Task<CompanyResponseDTO?> GetByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        return company?.Adapt<CompanyResponseDTO>();
    }

    public async Task<CompanyResponseDTO> CreateAsync(CompanyRequestDTO request)
    {
        _logger.LogInformation("Iniciando cadastro da empresa: {TradeName}", request.TradeName);

        var existingCompany = await _companyRepository.GetByCnpjAsync(request.Cnpj);
        if (existingCompany != null)
        {
            _logger.LogWarning("Tentativa de cadastro com CNPJ já existente: {Cnpj}", request.Cnpj);
            throw new DuplicateEntryException("Já existe uma empresa cadastrada com este CNPJ.");
        }

        await _cepService.EnsureValidCepAsync(request.Cep);

        var company = request.Adapt<Company>();
        await _companyRepository.AddAsync(company);

        return company.Adapt<CompanyResponseDTO>();
    }

    public async Task UpdateAsync(Guid id, CompanyRequestDTO request)
    {
        var company = await _companyRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Empresa não encontrada.");

        _logger.LogInformation("Atualizando dados da empresa {CompanyId}", id);

        if (company.Cep != request.Cep)
            await _cepService.EnsureValidCepAsync(request.Cep);

        request.Adapt(company);

        await _companyRepository.UpdateAsync(company);
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Executando exclusão lógica da empresa {CompanyId}", id);

        await _companyRepository.DeleteAsync(id);
    }

}