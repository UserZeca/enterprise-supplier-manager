using EnterpriseSupplierManager.Application.DTOs.Supplier;

namespace EnterpriseSupplierManager.Application.Interfaces;

public interface ISupplierService
{
    Task<SupplierResponseDTO> CreateAsync(SupplierRequestDTO request, Guid companyId);
    Task<IEnumerable<SupplierResponseDTO>> GetAllByCompanyIdAsync(Guid companyId);
}