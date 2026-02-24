using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.Models;
using ETSU_Marketplace.ViewModels;
using System.Linq;

namespace ETSU_Marketplace.Controllers
{
    public class HomeController : Controller
    {
        private HomeIndexViewModel BuildSampleVm()
        {
            var vm = new HomeIndexViewModel();

            vm.LatestItemListings.AddRange(new[]
            {
                new ListingCardViewModel { Id=1, Title="Calculus Textbook", ShortDescription="Good condition. No highlighting.", Price=35, CreatedAt=DateTime.Now.AddDays(-3), ListingType="Item", CategoryLabel="Textbook", ConditionLabel="Used - Good", DetailsUrl="#", ImageUrl="/images/placeholder.png" },
                new ListingCardViewModel { Id=2, Title="Gaming Headset", ShortDescription="Like new. Works perfectly.", Price=45, CreatedAt=DateTime.Now.AddDays(-2), ListingType="Item", CategoryLabel="Gaming", ConditionLabel="Like new", DetailsUrl="#", ImageUrl="/images/placeholder.png" },
                new ListingCardViewModel { Id=3, Title="Desk Lamp", ShortDescription="Bright LED lamp for dorm/desk.", Price=10, CreatedAt=DateTime.Now.AddDays(-1), ListingType="Item", CategoryLabel="Home", ConditionLabel="Used - Excellent", DetailsUrl="#", ImageUrl="/images/placeholder.png" },
                new ListingCardViewModel { Id=4, Title="Winter Jacket", ShortDescription="Size M. Warm and clean.", Price=25, CreatedAt=DateTime.Now.AddDays(-4), ListingType="Item", CategoryLabel="Clothing", ConditionLabel="Used - Good", DetailsUrl="#", ImageUrl="/images/placeholder.png" },
                new ListingCardViewModel { Id=5, Title="Car Phone Mount", ShortDescription="New in box.", Price=8, CreatedAt=DateTime.Now.AddDays(-5), ListingType="Item", CategoryLabel="Vehicles", ConditionLabel="Brand new", DetailsUrl="#", ImageUrl="/images/placeholder.png" }
            });

            vm.LatestLeaseListings.AddRange(new[]
            {
                new ListingCardViewModel { Id=101, Title="1BR Apartment Takeover", ShortDescription="Near campus. Utilities included.", Price=650, CreatedAt=DateTime.Now.AddDays(-2), ListingType="Lease", CategoryLabel="Lease", DetailsUrl="#", ImageUrl="/images/placeholder.png" },
                new ListingCardViewModel { Id=102, Title="Room in 3BR House", ShortDescription="Quiet roommates. Lease starts soon.", Price=520, CreatedAt=DateTime.Now.AddDays(-1), ListingType="Lease", CategoryLabel="Lease", DetailsUrl="#", ImageUrl="/images/placeholder.png" }
            });

            return vm;
        }

        public IActionResult Index()
        {
            var vm = BuildSampleVm();

            vm.LatestItemListings = vm.LatestItemListings
                .OrderByDescending(x => x.CreatedAt)
                .Take(4)
                .ToList();
            
            vm.LatestLeaseListings = vm.LatestLeaseListings
                .OrderByDescending(x => x.CreatedAt)
                .Take(4)
                .ToList();

            return View(vm);
        }
    }
}