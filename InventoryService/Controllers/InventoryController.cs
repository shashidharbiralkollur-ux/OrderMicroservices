using InventoryService.Models;
using InventoryService.Repository;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly InventoryRepository _repository;

    public InventoryController(InventoryRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("reserve")]
    public IActionResult Reserve(InventoryRequest request)
    {
        bool reserved = _repository.ReserveStock(request);

        if (!reserved)
            return BadRequest(new { message = "Insufficient stock" });

        return Ok(new
        {
            message = "Stock reserved",
            product = request.ProductName,
            quantity = request.Quantity
        });
    }
}
