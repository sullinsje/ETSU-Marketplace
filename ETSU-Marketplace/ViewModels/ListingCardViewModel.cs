namespace ETSU_Marketplace.ViewModels
{
    public class ListingCardViewModel
    {
        public int Id { get; set; }

        // Common fields
        public string Title { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSold { get; set; }

        // Favorites
        public bool IsFavorited { get; set; }

        // Display helpers
        public string ListingType { get; set; } = "";
        public string? CategoryLabel { get; set; }
        public string? ConditionLabel { get; set; }
        public bool ShowOwnerActions { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();
        public string ImageUrl => (ImageUrls != null && ImageUrls.Any())
            ? ImageUrls[0]
            : "/images/placeholder.png";

        public string DetailsUrl { get; set; } = "";
        public string Poster { get; set; } = "";
        public string PosterAvatar { get; set; } = "";
    }
}