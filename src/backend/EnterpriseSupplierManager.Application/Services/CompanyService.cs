using Mapster;
using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;

namespace EnterpriseSupplierManager.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly ICepService _cepService;

    public CompanyService(ICompanyRepository repository, ICepService cepService)
    {
        _repository = repository;
        _cepService = cepService;
    }

    public async Task<CompanyResponseDTO> CreateAsync(CompanyRequestDTO request)
    {
        // Validation CEP
        var address = await _cepService.GetAddressByCepAsync(request.Cep);
        if (address == null) throw new Exception("Invalid CEP.");

        var company = request.Adapt<Company>();
        await _repository.AddAsync(company);

        return company.Adapt<CompanyResponseDTO>();
    }
}