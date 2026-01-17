using EnterpriseSupplierManager.Domain.Entities.Common;

namespace EnterpriseSupplierManager.Domain.Entities;

public class Company : BaseEntity
{

    public string TradeName { get; set; } = string.Empty; // Nome Fantasia
    public string Cnpj { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}