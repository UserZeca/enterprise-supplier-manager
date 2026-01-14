using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Application.Mappings;
using EnterpriseSupplierManager.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSupplierManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Registro do Mapster
        
        services.RegisterMappings();

        #endregion

        #region Registro dos Services
        
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ICompanyService, CompanyService>();

        #endregion

        return services;
    }
}