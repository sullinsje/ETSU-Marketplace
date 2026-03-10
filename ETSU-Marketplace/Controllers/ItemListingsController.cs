using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;

namespace ETSU_Marketplace.Controllers
{
    // COMMENTED OUT FOR EASE OF TESTING 
    // [Authorize] 
    [Route("Listings/Items")]
    public class ItemListingsController : Controller
    {
        private readonly IItemListingRepository _itemRepo;

        public ItemListingsController(IItemListingRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        [HttpGet("")]
        public async Task<IActionResult> Items(
            string? category,
            string? condition,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            string? q)
        {
            var items = await _itemRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var item in items)
            {
                var paths = new List<string>();
                foreach (var image in item.Images)
                {
                    paths.Add(image.Path);
                }

                vms.Add(new ListingCardViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    ShortDescription = item.Description,
                    Price = item.Price,
                    CreatedAt = item.CreatedAt,
                    ListingType = "Item",
                    CategoryLabel = item.Category.ToString(),
                    ConditionLabel = item.Condition.ToString(),
                    ShowOwnerActions = true,
                    DetailsUrl = $"/Listings/Items/Details/{item.Id}",
                    ImageUrls = [.. paths]
                });
            }

            category = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
            condition = string.IsNullOrWhiteSpace(condition) ? null : condition.Trim();
            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();
            sort = string.IsNullOrWhiteSpace(sort) ? null : sort.Trim();

            if (category != null)
            {
                vms = vms
                    .Where(l => string.Equals(l.CategoryLabel, category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (condition != null)
            {
                vms = vms
                    .Where(l => string.Equals(l.ConditionLabel, condition, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (minPrice.HasValue)
            {
                vms = vms
                    .Where(l => l.Price >= minPrice.Value)
                    .ToList();
            }

            if (maxPrice.HasValue)
            {
                vms = vms
                    .Where(l => l.Price <= maxPrice.Value)
                    .ToList();
            }

            if (q != null)
            {
                vms = vms
                    .Where(l =>
                        (!string.IsNullOrWhiteSpace(l.Title) &&
                         l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(l.ShortDescription) &&
                         l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            vms = sort switch
            {
                "price_asc" => vms.OrderBy(l => l.Price).ToList(),
                "price_desc" => vms.OrderByDescending(l => l.Price).ToList(),
                _ => vms.OrderByDescending(l => l.CreatedAt).ToList()
            };

            ViewBag.SelectedCategory = category;
            ViewBag.SelectedCondition = condition;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Sort = sort;
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

            var paths = new List<string>();
            foreach (var image in item.Images)
            {
                paths.Add(image.Path);
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
                ConditionLabel = item.Condition.ToString(),
                ShowOwnerActions = true,
                ImageUrls = [.. paths]
            };

            return View(vm);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemRepo.ReadAsync(id);
            return View(item);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemRepo.ReadAsync(id);
            return View(item);
        }

        [Route("Manage")]
        public async Task<IActionResult> Manage()
        {
            var items = await _itemRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var item in items)
            {
                var paths = new List<string>();
                foreach (var image in item.Images)
                {
                    paths.Add(image.Path);
                }

                vms.Add(new ListingCardViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    ShortDescription = item.Description,
                    Price = item.Price,
                    CreatedAt = item.CreatedAt,
                    ListingType = "Item",
                    CategoryLabel = item.Category.ToString(),
                    ConditionLabel = item.Condition.ToString(),
                    ShowOwnerActions = true,
                    ImageUrls = [.. paths]
                });
            }

            foreach (var v in vms)
            {
                homeIndexVM.LatestItemListings.Add(v);
            }

            return View(homeIndexVM);
        }
    }
}