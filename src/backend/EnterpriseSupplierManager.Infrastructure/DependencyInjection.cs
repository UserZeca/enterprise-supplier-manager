using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Domain.Interfaces;
using EnterpriseSupplierManager.Infrastructure.Context;
using EnterpriseSupplierManager.Infrastructure.Repositories;
using EnterpriseSupplierManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSupplierManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Registro do DbContext
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        #endregion

        #region Registro dos Repositórios

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        #endregion

        #region APIs externas


        services.AddHttpClient("CepApi", c =>
        {
            c.BaseAddress = new Uri("https://viacep.com.br/ws/");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<ICepService, CepService>();

        #endregion

        return services;
    }
}