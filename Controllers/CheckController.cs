using Demo.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    
    private readonly IProductRepositoryLocal _localRepo;
    private readonly IProductRepositoryRemote _remoteRepo;


    public CheckController(IProductRepositoryLocal localRepo, IProductRepositoryRemote remoteRepo)
    {
        _localRepo = localRepo;
        _remoteRepo = remoteRepo;
    }

    [HttpGet("local")]
    public async Task<IActionResult> GetLocal()
    {
        var products = await _localRepo.GetAll();
        return Ok(products);
    }
    [HttpGet("remote")]
    public async Task<IActionResult> GetRemote()
    {
        var products = await _remoteRepo.GetAll();
        return Ok(products);
    }
}