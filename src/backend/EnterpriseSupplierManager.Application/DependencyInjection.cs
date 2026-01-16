using EnterpriseSupplierManager.Application.Interfaces;
using EnterpriseSupplierManager.Application.Mappings;
using EnterpriseSupplierManager.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EnterpriseSupplierManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Registro do Mapster
        
        services.RegisterMappings();

        #endregion

        #region Validadores de DTOs

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        #endregion

        #region Registro dos Services

        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ICompanyService, CompanyService>();

        #endregion

        return services;
    }
}