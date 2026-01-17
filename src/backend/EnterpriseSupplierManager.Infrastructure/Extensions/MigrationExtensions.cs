using EnterpriseSupplierManager.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSupplierManager.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            context.Database.Migrate();
            Console.WriteLine(">>> Database synchronized successfully via Extension Method!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>> Error during migration: {ex.Message}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($">>> Details: {ex.InnerException.Message}");
            }
        }
    }
}