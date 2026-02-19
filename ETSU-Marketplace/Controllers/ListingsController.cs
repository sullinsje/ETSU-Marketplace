using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;

namespace ETSU_Marketplace.Controllers
{
    // COMMENTED OUT FOR EASE OF TESTING 
    // [Authorize] 
    public class ListingsController : Controller
    {
        private readonly IItemListingRepository _itemRepo;

        public ListingsController(IItemListingRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        // Sprint 1: stub data so the UI works before EF Core is wired
        private HomeIndexViewModel BuildSampleVm()
        {
            var vm = new HomeIndexViewModel();

            // Item listings
            vm.LatestItemListings.AddRange(new[]
            {
                new ListingCardViewModel
                {
                    Id = 1,
                    Title = "Calculus Textbook",
                    ShortDescription = "Good condition. No highlighting.",
                    Price = 35,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    ListingType = "Item",
                    CategoryLabel = "Textbook",
                    ConditionLabel = "Used - Good",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                },
                new ListingCardViewModel
                {
                    Id = 2,
                    Title = "Gaming Headset",
                    ShortDescription = "Like new. Works perfectly.",
                    Price = 45,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ListingType = "Item",
                    CategoryLabel = "Gaming",
                    ConditionLabel = "Like new",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                },
                new ListingCardViewModel
                {
                    Id = 3,
                    Title = "Desk Lamp",
                    ShortDescription = "Bright LED lamp for dorm/desk.",
                    Price = 10,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ListingType = "Item",
                    CategoryLabel = "Home",
                    ConditionLabel = "Used - Excellent",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                },
                new ListingCardViewModel
                {
                    Id = 4,
                    Title = "Winter Jacket",
                    ShortDescription = "Size M. Warm and clean.",
                    Price = 25,
                    CreatedAt = DateTime.Now.AddDays(-4),
                    ListingType = "Item",
                    CategoryLabel = "Clothing",
                    ConditionLabel = "Used - Good",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                },
                new ListingCardViewModel
                {
                    Id = 5,
                    Title = "Car Phone Mount",
                    ShortDescription = "New in box.",
                    Price = 8,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    ListingType = "Item",
                    CategoryLabel = "Vehicles",
                    ConditionLabel = "Brand new",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                }
            });

            // Lease takeover listings
            vm.LatestLeaseListings.AddRange(new[]
            {
                new ListingCardViewModel
                {
                    Id = 101,
                    Title = "1BR Apartment Takeover",
                    ShortDescription = "Near campus. Utilities included.",
                    Price = 650,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ListingType = "Lease",
                    CategoryLabel = "Lease",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                },
                new ListingCardViewModel
                {
                    Id = 102,
                    Title = "Room in 3BR House",
                    ShortDescription = "Quiet roommates. Lease starts soon.",
                    Price = 520,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ListingType = "Lease",
                    CategoryLabel = "Lease",
                    DetailsUrl = "#",
                    ImageUrl = "/images/placeholder.png"
                }
            });

            return vm;
        }

        // GET: /Listings/Items?category=Textbook&q=calc
        public IActionResult Items(string? category, string? q)
        {
            var vm = BuildSampleVm();

            // Normalize inputs
            category = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

            // Filter by category if provided
            if (category != null)
            {
                vm.LatestItemListings = vm.LatestItemListings
                    .Where(l => string.Equals(l.CategoryLabel, category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by search query if provided
            if (q != null)
            {
                vm.LatestItemListings = vm.LatestItemListings
                    .Where(l =>
                        (!string.IsNullOrWhiteSpace(l.Title) && l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(l.ShortDescription) && l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            // Pass selected filters to the View (optional display)
            ViewBag.SelectedCategory = category;
            ViewBag.SearchQuery = q;

            return View(vm);
        }

        // GET: /Listings/Leases?q=apartment
        public IActionResult Leases(string? q)
        {
            var vm = BuildSampleVm();

            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

            if (q != null)
            {
                vm.LatestLeaseListings = vm.LatestLeaseListings
                    .Where(l =>
                        (!string.IsNullOrWhiteSpace(l.Title) && l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(l.ShortDescription) && l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.SearchQuery = q;

            return View(vm);
        }


        // GET: /Listings/Details/5?type=Item
        // GET: /Listings/Details/101?type=Lease
        public IActionResult Details(int id, string type = "Item")
        {
            var vm = BuildSampleVm();

            ListingCardViewModel? listing = null;

            if (string.Equals(type, "Lease", StringComparison.OrdinalIgnoreCase))
                listing = vm.LatestLeaseListings.FirstOrDefault(l => l.Id == id);
            else
                listing = vm.LatestItemListings.FirstOrDefault(l => l.Id == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var items = await _itemRepo.ReadAllAsync();
            return View(items);
        }

    }
}
