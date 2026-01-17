using Mapster;
using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSupplierManager.Application.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings(this IServiceCollection services)
    {
        // Supplier Configurations 
        TypeAdapterConfig<Supplier, SupplierResponseDTO>
            .NewConfig()
            .Map(dest => dest.BirthDate, src => src.BirthDate.HasValue
                ? src.BirthDate.Value.ToString("dd/MM/yyyy")
                : null);

        // Global Register Mapster
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfig).Assembly);
    }
}