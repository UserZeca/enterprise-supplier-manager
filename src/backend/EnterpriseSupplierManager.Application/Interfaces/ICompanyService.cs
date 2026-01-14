using EnterpriseSupplierManager.Application.DTOs.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSupplierManager.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDTO> CreateAsync(CompanyRequestDTO request);
    }
}
