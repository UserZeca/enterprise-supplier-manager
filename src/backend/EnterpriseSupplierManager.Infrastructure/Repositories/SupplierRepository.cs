using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;
using EnterpriseSupplierManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSupplierManager.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) => _context = context;


    public async Task<Supplier?> GetByDocumentAsync(string document)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Document == document);
    }

    public async Task AddAsync(Supplier supplier)
    {
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task AssociateToCompanyAsync(Guid supplierId, Guid companyId)
    {
        var supplier = await _context.Suppliers
            .Include(s => s.Companies)
            .FirstOrDefaultAsync(s => s.Id == supplierId);

        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (supplier != null && company != null)
        {
            if (!supplier.Companies.Any(c => c.Id == companyId))
            {
                supplier.Companies.Add(company);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<Supplier?> GetByIdAsync(Guid id) =>
        await _context.Suppliers.FindAsync(id);

    public async Task<IEnumerable<Supplier>> GetAllAsync() =>
      await _context.Suppliers.ToListAsync();

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

    public async Task<Supplier?> GetByIdWithCompaniesAsync(Guid id)
    {
        return await _context.Suppliers
            .Include(s => s.Companies)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _context.Suppliers
            .Where(s => s.Companies.Any(c => c.Id == companyId))
            .ToListAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier != null)
        {
            supplier.Delete();

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
    }

}