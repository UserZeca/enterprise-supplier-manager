using EnterpriseSupplierManager.Application.DTOs.Common;

namespace EnterpriseSupplierManager.Application.Interfaces;

public interface ICepService
{
    Task<PostalCodeResponseDTO?> GetAddressByCepAsync(string cep);
}