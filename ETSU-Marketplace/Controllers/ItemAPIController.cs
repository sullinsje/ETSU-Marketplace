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
public class ItemAPIController : ControllerBase
{
    private readonly IItemListingRepository _itemRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public ItemAPIController(IItemListingRepository itemRepo, UserManager<ApplicationUser> userManager)
    {
        _itemRepo = itemRepo;
        _userManager = userManager;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromForm] ItemListing item, List<IFormFile> images)
    {
        // Get current logged in user
        var userId = _userManager.GetUserId(User);

        if (userId == null) return Unauthorized();

        // send user and images
        await _itemRepo.CreateAsync(item, images, userId);
        return LocalRedirect("/Listings/Items/Manage");
    }

    [HttpGet("all")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _itemRepo.ReadAllAsync());
    }

    [HttpGet("one/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var itemListing = await _itemRepo.ReadAsync(id);
        if (itemListing == null)
        {
            return NotFound();
        }
        return Ok(itemListing);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Put([FromForm] ItemListing item, List<IFormFile> images)
    {
        await _itemRepo.UpdateAsync(item.Id, item, images);
        
        // Redirect back to management dashboard
        return LocalRedirect("/Listings/Items/Manage");
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await _itemRepo.DeleteAsync(id);

        // Redirect back to management dashboard
        return LocalRedirect("/Listings/Items/Manage");
    }

}
