using ETSU_Marketplace.Models;
using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETSU_Marketplace.Controllers;

[Route("api/[controller]")]
public class LeaseAPIController : BaseAPIController<LeaseListing, ILeaseListingRepository>
{
    public LeaseAPIController(ILeaseListingRepository leaseRepo, UserManager<ApplicationUser> userManager) 
        : base(leaseRepo, userManager) { }

    protected override string GetRedirectPath() => "/Listings/Leases/Manage";
}