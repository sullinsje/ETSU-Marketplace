using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[EnableCors]
[Route("api/[controller]")]
[ApiController]
public class LeaseAPIController : ControllerBase
{
    private readonly ILeaseListingRepository _leaseRepo;

    public LeaseAPIController(ILeaseListingRepository leaseRepo)
    {
        _leaseRepo = leaseRepo;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromForm] LeaseListing lease)
    {
        await _leaseRepo.CreateAsync(lease);
        return CreatedAtAction("Get", new { id = lease.Id }, lease);
    }

    [HttpGet("all")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _leaseRepo.ReadAllAsync());
    }

    [HttpGet("one/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var leaseListing = await _leaseRepo.ReadAsync(id);
        if (leaseListing == null)
        {
            return NotFound();
        }
        return Ok(leaseListing);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Put([FromForm] LeaseListing lease)
    {
        await _leaseRepo.UpdateAsync(lease.Id, lease);
        
        // Redirect back to management dashboard
        return LocalRedirect("/Listings/Leases/Manage");
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _leaseRepo.DeleteAsync(id);

        // Redirect back to management dashboard
        return LocalRedirect("/Listings/Leases/Manage");
    }

}
