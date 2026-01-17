using EnterpriseSupplierManager.Domain.Entities;

namespace EnterpriseSupplierManager.Domain.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByDocumentAsync(string document);
    Task AddAsync(Supplier supplier);
    Task AssociateToCompanyAsync(Guid supplierId, Guid companyId);
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdWithCompaniesAsync(Guid id);
    Task<IEnumerable<Supplier>> GetByCompanyIdAsync(Guid companyId);
    Task AddWithCompanyAsync(Supplier supplier, Guid companyId);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(Guid id);
}