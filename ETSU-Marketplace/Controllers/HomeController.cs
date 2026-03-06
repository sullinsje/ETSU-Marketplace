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
                .Include(x => x.Images)
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
                    DetailsUrl = $"/Listings/Items/Details/{x.Id}?type=Item",
                    ImageUrls = x.Images.Select(i => i.Path).ToList(),
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
                    ImageUrls = x.Images.Select(img => img.Path).ToList(),
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

        public IActionResult Search(string? q)
        {
            if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction("Index");

            const int take = 200;

            var items = _db.ItemListings
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .Select(x => new ListingCardViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.Description,
                    Price = x.Price,
                    ImageUrls = x.Images.OrderBy(i => i.Id).Select(i => i.Path.StartsWith("/") ? i.Path : "/" + i.Path).ToList(),
                    CreatedAt = x.CreatedAt,
                    ListingType = "Item",
                    DetailsUrl = $"/Listings/Items/Details/{x.Id}?type=Item"
                })
                .ToList();

            var leases = _db.LeaseListings
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .Select(x => new ListingCardViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.Description,
                    Price = x.Price,
                    ImageUrls = x.Images.OrderBy(i => i.Id).Select(i => i.Path.StartsWith("/") ? i.Path : "/" + i.Path).ToList(),
                    CreatedAt = x.CreatedAt,
                    ListingType = "Lease",
                    DetailsUrl = $"/Listings/Leases/Details/{x.Id}?type=Lease"
                })
                .ToList();

            ViewBag.SearchQuery = q;

            return View("SearchResults", new HomeIndexViewModel
            {
                LatestItemListings = items,
                LatestLeaseListings = leases
            });
        }
    }
}