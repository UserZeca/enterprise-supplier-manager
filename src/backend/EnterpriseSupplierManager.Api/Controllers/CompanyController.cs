using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSupplierManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponseDTO>> Create(CompanyRequestDTO request)
    {
        try
        {
            var result = await _companyService.CreateAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}