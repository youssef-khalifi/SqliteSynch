using Demo.Features;
using Demo.Features.CreateProduct;
using Demo.Features.DeleteProduct;
using Demo.Features.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetAllProductCommand();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand createProductCommand)
    {
        var result = await _mediator.Send(createProductCommand);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteProductCommand deleteProductCommand)
    {
        var result = await _mediator.Send(deleteProductCommand);

        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    
}