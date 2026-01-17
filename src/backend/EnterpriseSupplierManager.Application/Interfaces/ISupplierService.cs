using EnterpriseSupplierManager.Application.DTOs.Supplier;

namespace EnterpriseSupplierManager.Application.Interfaces;

public interface ISupplierService
{
    Task<SupplierResponseDTO> CreateAsync(SupplierRequestDTO request);
    Task<IEnumerable<SupplierResponseDTO>> GetAllByCompanyIdAsync(Guid companyId);
    Task UpdateAsync(Guid id, SupplierRequestDTO request);
    Task DeleteAsync(Guid id);
    Task<SupplierResponseDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<SupplierResponseDTO>> GetAllAsync();
    Task AssociateToCompanyAsync(Guid supplierId, Guid companyId);
}