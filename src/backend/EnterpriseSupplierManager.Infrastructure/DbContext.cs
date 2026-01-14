using EnterpriseSupplierManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EnterpriseSupplierManager.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Company
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TradeName).IsRequired().HasMaxLength(150);

            // Unique CNPJ
            entity.HasIndex(e => e.Cnpj).IsUnique();
            entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(14);

            entity.Property(e => e.Uf).IsRequired().HasMaxLength(2);
            entity.Property(e => e.Cep).IsRequired().HasMaxLength(8);
        });

        // Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);

            // Unique CPF/CNPJ
            entity.HasIndex(s => s.Document).IsUnique();
            entity.Property(s => s.Document).IsRequired().HasMaxLength(14);

            entity.Property(s => s.Email).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Cep).IsRequired().HasMaxLength(8);

            entity.Property(s => s.Rg).HasMaxLength(20);
        });

        // Many-to-Many (N:N) Relationship
        modelBuilder.Entity<Company>()
            .HasMany(c => c.Suppliers)
            .WithMany(s => s.Companies)
            .UsingEntity(j => j.ToTable("CompanySuppliers"));
    }

}