using ETSU_Marketplace.Models;
using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[EnableCors]
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LeaseAPIController : ControllerBase
{
    private readonly ILeaseListingRepository _leaseRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public LeaseAPIController(ILeaseListingRepository leaseRepo, UserManager<ApplicationUser> userManager)
    {
        _leaseRepo = leaseRepo;
        _userManager = userManager;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromForm] LeaseListing lease, List<IFormFile> images)
    {
        // Get current logged in user
        var userId = _userManager.GetUserId(User);

        if (userId == null) return Unauthorized();

        // send user and images
        await _leaseRepo.CreateAsync(lease, images, userId);
        return LocalRedirect("/Listings/Leases/Manage");
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
    public async Task<IActionResult> Put([FromForm] LeaseListing lease, List<IFormFile> images)
    {
        await _leaseRepo.UpdateAsync(lease.Id, lease, images);

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
