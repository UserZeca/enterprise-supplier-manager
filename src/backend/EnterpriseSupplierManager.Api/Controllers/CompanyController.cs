using EnterpriseSupplierManager.Application.DTOs.Company;
using EnterpriseSupplierManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSupplierManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyResponseDTO>>> GetAll()
    {
        var companies = await _companyService.GetAllAsync();
        return Ok(companies);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompanyResponseDTO>> GetById(Guid id)
    {
        var company = await _companyService.GetByIdAsync(id);

        if (company == null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponseDTO>> Create([FromBody] CompanyRequestDTO request)
    {
        var result = await _companyService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CompanyRequestDTO request)
    {
        await _companyService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _companyService.DeleteAsync(id);
        return NoContent();
    }
}