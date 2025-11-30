using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Firmeza.Api.DTOs;
using Firmeza.Api.Services;
using Firmeza.Data.Models;

namespace Firmeza.Api.Controllers;

/// <summary>
/// Controller for Sale CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    public SalesController(ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all sales (Admin only)
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <returns>List of sales</returns>
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var sales = await _saleService.GetAllSalesAsync(page, pageSize);
        var saleDtos = _mapper.Map<List<SaleDto>>(sales);
        return Ok(saleDtos);
    }

    /// <summary>
    /// Get a specific sale by ID
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <returns>Sale details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);

        if (sale == null)
        {
            return NotFound(new { message = "Sale not found" });
        }

        var saleDto = _mapper.Map<SaleDto>(sale);
        return Ok(saleDto);
    }

    /// <summary>
    /// Get all sales for a specific customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>List of customer sales</returns>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByCustomer(int customerId)
    {
        var sales = await _saleService.GetSalesByCustomerIdAsync(customerId);
        var saleDtos = _mapper.Map<List<SaleDto>>(sales);
        return Ok(saleDtos);
    }

    /// <summary>
    /// Create a new sale
    /// </summary>
    /// <param name="createSaleDto">Sale data with details</param>
    /// <returns>Created sale</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto createSaleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var sale = await _saleService.CreateSaleAsync(createSaleDto);
            var saleDto = _mapper.Map<SaleDto>(sale);
            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, saleDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing sale (Admin only)
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <param name="updateSaleDto">Updated sale data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale(int id, [FromBody] UpdateSaleDto updateSaleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _saleService.UpdateSaleAsync(id, updateSaleDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Sale not found" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a sale (Admin only)
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale(int id)
    {
        try
        {
            await _saleService.DeleteSaleAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Sale not found" });
        }
    }
}
