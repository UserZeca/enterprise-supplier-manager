using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSupplierManager.Application.DTOs.Supplier
{
    public class SupplierRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty; // CPF or CNPJ
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
