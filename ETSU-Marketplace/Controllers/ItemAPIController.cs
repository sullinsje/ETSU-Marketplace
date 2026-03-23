using ETSU_Marketplace.Models;
using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETSU_Marketplace.Controllers;

[Route("api/[controller]")]
public class ItemAPIController : BaseAPIController<ItemListing, IItemListingRepository>
{
    public ItemAPIController(IItemListingRepository itemRepo, UserManager<ApplicationUser> userManager) 
        : base(itemRepo, userManager) { }

    protected override string GetRedirectPath() => "/Listings/Items/Manage";
    
}