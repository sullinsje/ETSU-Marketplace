using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;

namespace ETSU_Marketplace.Controllers
{
    // COMMENTED OUT FOR EASE OF TESTING 
    // [Authorize] 
    [Route("Listings/Leases")]
    public class LeaseListingsController : Controller
    {
        private readonly ILeaseListingRepository _leaseRepo;

        public LeaseListingsController(ILeaseListingRepository leaseRepo)
        {
            _leaseRepo = leaseRepo;
        }

        // GET: /Listings/Leases?q=apartment
        public async Task<IActionResult> Leases(string? q)
        {
            var items = await _leaseRepo.ReadAllAsync();
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
                    ListingType = "Item"
                });
            }

            // Normalize inputs
            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

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
            ViewBag.SearchQuery = q;
            
            foreach (var v in vms)
            {
                homeIndexVM.LatestLeaseListings.Add(v);
            }
            return View(homeIndexVM);
        }

        [Route("Details/{id}")]
        // GET: /Listings/Details/101?type=Lease
        public async Task<IActionResult> Details(int id)
        {
            var item = await _leaseRepo.ReadAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }

            var vm = new ListingCardViewModel{
                Id = id,
                Title = item.Title,
                ShortDescription = item.Description,
                Price = item.Price,
                CreatedAt = item.CreatedAt,
                ListingType = "Item"
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
