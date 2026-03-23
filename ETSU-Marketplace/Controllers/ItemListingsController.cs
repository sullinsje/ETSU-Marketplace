using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Controllers
{
    [Authorize]
    [Route("Listings/Items/")]
    public class ItemListingsController : BaseListingsController<ItemListing, IItemListingRepository>
    {
        public ItemListingsController(IItemListingRepository itemRepo, UserManager<ApplicationUser> userManager)
            : base(itemRepo, userManager) { }

        [HttpGet("")]
        public async Task<IActionResult> Items(
            string? category,
            string? condition,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            string? q)
        {
            var items = await _repository.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var item in items)
            {
                var paths = new List<string>();
                foreach (var image in item.Images)
                {
                    paths.Add(image.Path);
                }

                // Use BaseListingController method to build VM
                var vm = MapToCardViewModel(item, false);
                vm.ListingType = "Item";
                vm.CategoryLabel = item.Category.ToString();
                vm.ConditionLabel = item.Condition.ToString();
                vm.DetailsUrl = $"/Listings/Items/Details/{item.Id}?type=Item";
                vms.Add(vm);
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
            var item = await _repository.ReadAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var paths = new List<string>();
            foreach (var image in item.Images)
            {
                paths.Add(image.Path);
            }

            // Use BaseListingController method to build VM
            var vm = MapToCardViewModel(item, false);
            vm.ListingType = "Item";
            vm.CategoryLabel = item.Category.ToString();
            vm.ConditionLabel = item.Condition.ToString();
            vm.Poster = $"{item.User!.FirstName} {item.User.LastName}";
            vm.PosterAvatar = item.User.Avatar.Path;

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
            // Allow only creators to access 
            if (CurrentUserId == null) return Unauthorized();
            var item = await _repository.ReadAsync(id);

            if (item == null) return NotFound();

            if (!await IsOwner(item))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(item);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Allow only creators to access 
            if (CurrentUserId == null) return Unauthorized();
            var item = await _repository.ReadAsync(id);

            if (item == null) return NotFound();

            if (!await IsOwner(item))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(item);
        }

        [Route("Manage")]
        public async Task<IActionResult> Manage()
        {
            if (CurrentUserId == null) return Unauthorized();

            var items = await _repository.ReadUserPostsAsync(CurrentUserId);
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var item in items)
            {

                var paths = new List<string>();
                foreach (var image in item.Images)
                {
                    paths.Add(image.Path);
                }

                // Use BaseListingController method to build VM
                // Set to true since this is only the current user's posts and they need to see 
                // owner actions
                var vm = MapToCardViewModel(item, true);
                vm.ListingType = "Item";
                vm.CategoryLabel = item.Category.ToString();
                vm.ConditionLabel = item.Condition.ToString();
                vm.DetailsUrl = $"/Listings/Items/Details/{item.Id}?type=Item";
                vms.Add(vm);
            }

            foreach (var v in vms)
            {
                homeIndexVM.LatestItemListings.Add(v);
            }

            return View(homeIndexVM);
        }
    }
}