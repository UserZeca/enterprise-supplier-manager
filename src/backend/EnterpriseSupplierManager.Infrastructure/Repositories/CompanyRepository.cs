using EnterpriseSupplierManager.Domain.Entities;
using EnterpriseSupplierManager.Domain.Interfaces;
using EnterpriseSupplierManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSupplierManager.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context) => _context = context;

    public async Task<Company?> GetByIdAsync(Guid id) =>
        await _context.Companies.FindAsync(id);

    public async Task<Company?> GetByCnpjAsync(string cnpj)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }

    public async Task<IEnumerable<Company>> GetAllAsync() =>
        await _context.Companies.ToListAsync();

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company != null)
        {
            company.Delete();

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }
    }

}