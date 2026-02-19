using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;

namespace ETSU_Marketplace.Controllers
{
    // COMMENTED OUT FOR EASE OF TESTING 
    // [Authorize] 
    [Route("Listings/Items/")]
    public class ItemListingsController : Controller
    {
        private readonly IItemListingRepository _itemRepo;

        public ItemListingsController(IItemListingRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        // GET: /Listings/Items?category=Textbook&q=calc
        public async Task<IActionResult> Items(string? category, string? q)
        {
            var items = await _itemRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();
            
            foreach (var item in items)
            {
                vms.Add(new ListingCardViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    ShortDescription = item.Description,
                    Price = item.Price,
                    CreatedAt = item.CreatedAt,
                    ListingType = "Item",
                    CategoryLabel = item.Category.ToString(),
                    ConditionLabel = item.Condition.ToString()
                });
            }

            // Normalize inputs
            category = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

            // Filter by category if provided
            if (category != null)
            {
                vms = vms
                    .Where(l => string.Equals(l.CategoryLabel, category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by search query if provided
            if (q != null)
            {
                vms = vms
                    .Where(l =>
                        (!string.IsNullOrWhiteSpace(l.Title) && l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(l.ShortDescription) && l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            // Pass selected filters to the View (optional display)
            ViewBag.SelectedCategory = category;
            ViewBag.SearchQuery = q;
            
            foreach (var v in vms)
            {
                homeIndexVM.LatestItemListings.Add(v);
            }
            return View(homeIndexVM);
        }


        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepo.ReadAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var vm = new ListingCardViewModel
            {
                Id = id,
                Title = item.Title,
                ShortDescription = item.Description,
                Price = item.Price,
                CreatedAt = item.CreatedAt,
                ListingType = "Item",
                CategoryLabel = item.Category.ToString(),
                ConditionLabel = item.Condition.ToString()
            };

            return View(vm);

        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

    }
}
