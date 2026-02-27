using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETSU_Marketplace.ViewModels;

namespace ETSU_Marketplace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // tune these counts to whatever you want on the homepage
            const int take = 8;

            var latestItems = _db.ItemListings
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .Select(x => new ListingCardViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.Description,
                    Price = x.Price,
                    CreatedAt = x.CreatedAt,
                    ListingType = "Item",
                    CategoryLabel = x.Category.ToString(),
                    ConditionLabel = x.Condition.ToString(),
                    ImageUrl = null,
                    DetailsUrl = $"/Listings/Items/Details/{x.Id}?type=Item"
                })
                .ToList();

            var latestLeases = _db.LeaseListings
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .Select(x => new ListingCardViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.Description,
                    Price = x.Price,
                    CreatedAt = x.CreatedAt,
                    ListingType = "Lease",
                    CategoryLabel = "Lease",
                    ConditionLabel = null,
                    ImageUrl = null,
                    DetailsUrl = $"/Listings/Leases/Details/{x.Id}?type=Lease"
                })
                .ToList();

            var vm = new HomeIndexViewModel
            {
                LatestItemListings = latestItems,
                LatestLeaseListings = latestLeases
            };

            return View(vm);
        }
    }
}