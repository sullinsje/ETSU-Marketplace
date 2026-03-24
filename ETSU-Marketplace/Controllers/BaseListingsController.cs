using ETSU_Marketplace.Models;
using ETSU_Marketplace.Services;
using ETSU_Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETSU_Marketplace.Controllers;

public abstract class BaseListingsController<TEntity, TRepository> : Controller
    where TEntity : Listing
    where TRepository : IListingRepository<TEntity>
{
    protected readonly TRepository _repository;
    protected readonly UserManager<ApplicationUser> _userManager;

    protected BaseListingsController(TRepository repository, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    protected string? CurrentUserId => _userManager.GetUserId(User);

    protected async Task<bool> IsOwner(TEntity entity)
    {
        return entity != null && entity.UserId == CurrentUserId;
    }

    protected ListingCardViewModel MapToCardViewModel(TEntity entity, bool showOwnerActions = false)
    {
        return new ListingCardViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            ShortDescription = entity.Description,
            Price = entity.Price,
            CreatedAt = entity.CreatedAt,
            ShowOwnerActions = showOwnerActions,
            ImageUrls = entity.Images.Select(i => i.Path).ToList(),
            
            Poster = $"{entity.User!.FirstName} {entity.User.LastName}",
            PosterAvatar = entity.User.Avatar.Path,
            PosterId = entity.User.Id
        };
    }
}