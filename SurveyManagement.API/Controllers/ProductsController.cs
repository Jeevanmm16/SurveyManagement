using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogTrace("Trace: Fetching all products");
        _logger.LogInformation("Fetching all products");

        try
        {
            var products = await _service.GetAllProductsAsync();
            _logger.LogInformation($"Returned {products?.Count() ?? 0} products");
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all products");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation($"Fetching product with ID: {id}");
        try
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Product with ID {id} returned successfully.");
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching product with ID {id}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateProductDto dto)
    {
        _logger.LogInformation($"Adding new product: {dto.ProductName}");
        try
        {
            var created = await _service.AddProductAsync(dto);
            _logger.LogInformation($"Product created with ID: {created.ProductId}");
            return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        _logger.LogInformation($"Updating product ID: {id}");
        try
        {
            var updated = await _service.UpdateProductAsync(id, dto);
            _logger.LogInformation($"Product ID {id} updated successfully.");
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating product ID: {id}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation($"Deleting product ID: {id}");
        try
        {
            await _service.DeleteProductAsync(id);
            _logger.LogInformation($"Product ID {id} deleted successfully.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product ID: {id}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
