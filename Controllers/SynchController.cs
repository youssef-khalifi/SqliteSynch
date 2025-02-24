using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SynchController : ControllerBase
{
    private readonly SynchronizationService _synchronizationService;

    public SynchController(SynchronizationService synchronizationService)
    {
        _synchronizationService = synchronizationService;
    }

    [HttpPost("synch")]
    public async Task<IActionResult> Get()
    {
        await _synchronizationService.SyncDatabases();
        return Ok();
    }
}