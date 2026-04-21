using ETSU_Marketplace.Models;
using ETSU_Marketplace.Services;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// The users controller handles user profile pages, allowing users
/// to view public user details and displaying the currently authenticated
/// user's profile management page.
/// </summary>

namespace ETSU_Marketplace.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepo;

        public UsersController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepo)
        {
            _db = db;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepo.ReadProfileAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [Route("Manage")]
        public async Task<IActionResult> Manage()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var user = await _userRepo.ReadProfileAsync(userId);

            return View(user);
        }

    }
}