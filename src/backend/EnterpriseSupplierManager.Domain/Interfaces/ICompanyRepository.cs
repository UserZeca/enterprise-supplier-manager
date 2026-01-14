using EnterpriseSupplierManager.Domain.Entities;

namespace EnterpriseSupplierManager.Domain.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task AddAsync(Company company);
}