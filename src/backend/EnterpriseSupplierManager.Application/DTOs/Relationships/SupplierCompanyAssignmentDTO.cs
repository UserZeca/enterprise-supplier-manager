using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSupplierManager.Application.DTOs.Relationships
{
    public class SupplierCompanyAssignmentDTO
    {
        public Guid CompanyId { get; set; }
        public Guid SupplierId { get; set; }
    }
}
