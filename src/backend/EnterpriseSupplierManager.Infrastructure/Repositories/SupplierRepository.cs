using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;
using EnterpriseSupplierManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSupplierManager.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) => _context = context;

    public async Task AddWithCompanyAsync(Supplier supplier, Guid companyId)
    {
        var company = await _context.Companies.FindAsync(companyId);

        if (company != null)
        {
            supplier.Companies ??= new List<Company>();
            supplier.Companies.Add(company);

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Supplier>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _context.Suppliers
            .Where(s => s.Companies.Any(c => c.Id == companyId))
            .ToListAsync();
    }
}