using Microsoft.AspNetCore.Identity;

namespace ETSU_Marketplace.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public int? AvatarId { get; set; }
    public Image Avatar {get; set;} = new Image {Path = "/images/default-avatar.jpg"};
    public ICollection<Listing> Listings {get; set;} = new List<Listing>();
    
}