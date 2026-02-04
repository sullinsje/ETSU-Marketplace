using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.Models;
using ETSU_Marketplace.ViewModels;

namespace ETSU_Marketplace.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var vm = new HomeIndexViewModel
            {
                LatestItemListings =
                {
                    new ListingCardViewModel
                    {
                        Id = 1,
                        Title = "Laptop for Sale",
                        ShortDescription = "Good condition laptop, includes charger.",
                        Price = 400,
                        CreatedAt = DateTime.Now.AddDays(-2),
                        ListingType = "Item",
                        CategoryLabel = "Electronics",
                        ConditionLabel = "Used - Good",
                        DetailsUrl = "/Listings/Details/1"
                    }
                },
                LatestLeaseListings =
                {
                    new ListingCardViewModel
                    {
                        Id = 101,
                        Title = "1BR Apartment Takeover",
                        ShortDescription = "Near campus. Utilities included.",
                        Price = 600,
                        CreatedAt = DateTime.Now.AddDays(-1),
                        ListingType = "Lease",
                        CategoryLabel = "Housing",
                        DetailsUrl = "/LeaseListings/Details/101"
                    }
                }
            };

            return View(vm);
        }
    }
}