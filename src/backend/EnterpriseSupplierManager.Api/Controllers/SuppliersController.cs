using EnterpriseSupplierManager.Application.DTOs.Supplier;
using EnterpriseSupplierManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSupplierManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(ISupplierService supplierService, ILogger<SuppliersController> logger)
    {
        _supplierService = supplierService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponseDTO>> Create([FromBody] SupplierRequestDTO request)
    {
        var result = await _supplierService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SupplierRequestDTO request)
    {
        await _supplierService.UpdateAsync(id, request);
        return NoContent(); 
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _supplierService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierResponseDTO>> GetById(Guid id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);

        if (supplier == null)
            return NotFound(new { message = "Fornecedor não encontrado." });

        return Ok(supplier);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierResponseDTO>>> GetAll()
    {
        var result = await _supplierService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost("{supplierId:guid}/associate/{companyId:guid}")]
    public async Task<IActionResult> Associate(Guid supplierId, Guid companyId)
    {
        await _supplierService.AssociateToCompanyAsync(supplierId, companyId);
        return Ok(new { message = "Fornecedor vinculado com sucesso e aprovado nas regras de compliance." });
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<ActionResult<IEnumerable<SupplierResponseDTO>>> GetByCompany(Guid companyId)
    {
        var suppliers = await _supplierService.GetAllByCompanyIdAsync(companyId);
        return Ok(suppliers);
    }
}