using Mapster;
using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;

namespace EnterpriseSupplierManager.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICompanyRepository _companyRepository;

    public SupplierService(ISupplierRepository supplierRepository, ICompanyRepository companyRepository)
    {
        _supplierRepository = supplierRepository;
        _companyRepository = companyRepository;
    }

    public async Task<SupplierResponseDTO> CreateAsync(SupplierRequestDTO request, Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId)
            ?? throw new Exception("Company not found.");

        // Implementation of the Paraná Rule
        ValidateParanaRule(request, company.Uf);

        var supplier = request.Adapt<Supplier>();

        await _supplierRepository.AddWithCompanyAsync(supplier, companyId);

        return supplier.Adapt<SupplierResponseDTO>();
    }

    private void ValidateParanaRule(SupplierRequestDTO request, string companyUf)
    {

        if (companyUf.ToUpper() == "PR" && IsPhysicalPerson(request.Document))
        {
            if (!request.BirthDate.HasValue)
                throw new Exception("Birth date is required for physical persons in Paraná.");

            var age = DateTime.Today.Year - request.BirthDate.Value.Year;
            if (request.BirthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
                throw new Exception("Suppliers from individual persons in Paraná must be 18 years or older.");
        }
    }

    private bool IsPhysicalPerson(string document) => document.Length == 11; // Lógica simples de CPF

    public async Task<IEnumerable<SupplierResponseDTO>> GetAllByCompanyIdAsync(Guid companyId)
    {
        var suppliers = await _supplierRepository.GetByCompanyIdAsync(companyId);
        return suppliers.Adapt<IEnumerable<SupplierResponseDTO>>();
    }
}