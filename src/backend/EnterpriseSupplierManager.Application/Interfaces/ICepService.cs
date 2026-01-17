using EnterpriseSupplierManager.Application.DTOs.Common;

namespace EnterpriseSupplierManager.Application.Interfaces;

public interface ICepService
{
    Task EnsureValidCepAsync(string cep);
    Task<CepResponseDTO?> GetAddressByCepAsync(string cep);
}