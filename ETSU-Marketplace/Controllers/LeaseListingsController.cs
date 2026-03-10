using Microsoft.AspNetCore.Mvc;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ETSU_Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Controllers
{
    // COMMENTED OUT FOR EASE OF TESTING 
    [Authorize] 
    [Route("Listings/Leases")]
    public class LeaseListingsController : Controller
    {
        private readonly ILeaseListingRepository _leaseRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaseListingsController(ILeaseListingRepository leaseRepo, UserManager<ApplicationUser> userManager)
        {
            _leaseRepo = leaseRepo;
            _userManager = userManager;
        }

        // GET: /Listings/Leases?q=apartment
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
                    DetailsUrl = $"/Listings/Leases/Details/{lease.Id}",
                    ImageUrls = [.. paths]
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
                ImageUrls = [.. paths],
                Poster = $"{lease.User!.FirstName} {lease.User.LastName}",
                PosterAvatar = lease.User.Avatar.Path
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
            // Allow only creators to access 
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var lease = await _leaseRepo.ReadAsync(id);

            if (lease == null) return NotFound();

            if (lease.UserId != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(lease);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Allow only creators to access 
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();
            var lease = await _leaseRepo.ReadAsync(id);

            if (lease == null) return NotFound();

            if (lease.UserId != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(lease);
        }

        [Route("Manage")]
        public async Task<IActionResult> Manage()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            // fetch only the logged in user's post
            var leases = await _leaseRepo.ReadUserPostsAsync(userId);
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
                    ImageUrls = [.. paths]
                });
            }

            foreach (var v in vms)
            {
                homeIndexVM.LatestLeaseListings.Add(v);
            }
            return View(homeIndexVM);
        }
    }
}
