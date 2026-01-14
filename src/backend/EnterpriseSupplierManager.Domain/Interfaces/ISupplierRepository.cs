using EnterpriseSupplierManager.Domain.Entities;

namespace EnterpriseSupplierManager.Domain.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetByCompanyIdAsync(Guid companyId);
    Task AddWithCompanyAsync(Supplier supplier, Guid companyId);
}