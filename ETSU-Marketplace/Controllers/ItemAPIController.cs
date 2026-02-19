using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[EnableCors]
[Route("api/[controller]")]
[ApiController]
public class ItemAPIController : ControllerBase
{
    private readonly IItemListingRepository _itemRepo;

    public ItemAPIController(IItemListingRepository itemRepo)
    {
        _itemRepo = itemRepo;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromForm] ItemListing item)
    {
        await _itemRepo.CreateAsync(item);
        return CreatedAtAction("Get", new { id = item.Id }, item);
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

    [HttpPut("update")]
    public async Task<IActionResult> Put([FromForm] ItemListing item)
    {
        await _itemRepo.UpdateAsync(item.Id, item);
        return NoContent(); // 204 as per HTTP specification
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _itemRepo.DeleteAsync(id);
        return NoContent(); // 204 as per HTTP specification
    }

}
