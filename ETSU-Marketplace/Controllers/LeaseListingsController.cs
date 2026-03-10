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

        public async Task<IActionResult> Leases(string? q)
        {
            var leases = await _leaseRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var lease in leases)
            {
                var paths = new List<string>();

                foreach (var image in lease.Images)
                {
                    paths.Add(image.Path);
                }

                vms.Add(new ListingCardViewModel
                {
                    Id = lease.Id,
                    Title = lease.Title,
                    ShortDescription = lease.Description,
                    Price = lease.Price,
                    CreatedAt = lease.CreatedAt,
                    ListingType = "Lease",
                    ShowOwnerActions = true,
                    DetailsUrl = $"/Listings/Leases/Details/{lease.Id}?type=Lease",
                    ImageUrls = [.. paths]
                });
            }

            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

            if (q != null)
            {
                vms = vms
                    .Where(l =>
                        (!string.IsNullOrWhiteSpace(l.Title) && l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(l.ShortDescription) && l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.SearchQuery = q;

            foreach (var v in vms)
            {
                homeIndexVM.LatestLeaseListings.Add(v);
            }
            return View(homeIndexVM);
        }

        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var lease = await _leaseRepo.ReadAsync(id);

            if (lease == null)
            {
                return NotFound();
            }

            var paths = new List<string>();
            foreach (var image in lease.Images)
            {
                paths.Add(image.Path);
            }

            var vm = new ListingCardViewModel
            {
                Id = id,
                Title = lease.Title,
                ShortDescription = lease.Description,
                Price = lease.Price,
                CreatedAt = lease.CreatedAt,
                ListingType = "Lease",
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
            var lease = await _leaseRepo.ReadAsync(id);
            return View(lease);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lease = await _leaseRepo.ReadAsync(id);
            return View(lease);
        }

        [Route("Manage")]
        public async Task<IActionResult> Manage()
        {
            var leases = await _leaseRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var lease in leases)
            {
                var paths = new List<string>();

                foreach (var image in lease.Images)
                {
                    paths.Add(image.Path);
                }

                vms.Add(new ListingCardViewModel
                {
                    Id = lease.Id,
                    Title = lease.Title,
                    ShortDescription = lease.Description,
                    Price = lease.Price,
                    CreatedAt = lease.CreatedAt,
                    ListingType = "Lease",
                    ShowOwnerActions = true,
                    DetailsUrl = $"/Listings/Leases/Details/{lease.Id}?type=Lease",
                    ImageUrls = [.. paths]
                });
            }

            foreach (var v in vms)
            {
                homeIndexVM.LatestLeaseListings.Add(v);
            }
            return View(homeIndexVM);
        }

        [HttpGet("")]
        public async Task<IActionResult> Leases(decimal? minPrice, decimal? maxPrice, string? sort, string? q)
        {
            var leases = await _leaseRepo.ReadAllAsync();
            var vms = new List<ListingCardViewModel>();
            var homeIndexVM = new HomeIndexViewModel();

            foreach (var lease in leases)
            {
                var paths = new List<string>();

                foreach (var image in lease.Images)
                {
                    paths.Add(image.Path);
                }

                vms.Add(new ListingCardViewModel
                {
                    Id = lease.Id,
                    Title = lease.Title,
                    ShortDescription = lease.Description,
                    Price = lease.Price,
                    CreatedAt = lease.CreatedAt,
                    ListingType = "Lease",
                    ShowOwnerActions = true,
                    DetailsUrl = $"/Listings/Leases/Details/{lease.Id}?type=Lease",
                    ImageUrls = [.. paths]
                });
            }

            q = string.IsNullOrWhiteSpace(q) ? null : q.Trim();
            sort = string.IsNullOrWhiteSpace(sort) ? null : sort.Trim();

            if (minPrice.HasValue)
            {
                vms = vms.Where(l => l.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                vms = vms.Where(l => l.Price <= maxPrice.Value).ToList();
            }

            if (q != null)
            {
                vms = vms.Where(l => (!string.IsNullOrWhiteSpace(l.Title) && l.Title.Contains(q, StringComparison.OrdinalIgnoreCase)) || (!string.IsNullOrWhiteSpace(l.ShortDescription) && l.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            vms = sort switch
            {
                "price_asc" => vms.OrderBy(l => l.Price).ToList(),
                "price_desc" => vms.OrderByDescending(l => l.Price).ToList(),
                _ => vms.OrderByDescending(l => l.CreatedAt).ToList()
            };

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Sort = sort;
            ViewBag.SearchQuery = q;

            foreach (var v in vms)
            {
                homeIndexVM.LatestLeaseListings.Add(v);
            }

            return View(homeIndexVM);
        }
    }
}
