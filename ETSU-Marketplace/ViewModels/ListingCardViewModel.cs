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

        // Display helpers
        public string ListingType { get; set; } = "";   // "Item" or "Lease"
        public string? CategoryLabel { get; set; }      // e.g. "Electronics"
        public string? ConditionLabel { get; set; }     // e.g. "Like new"

        // Optional for later
        public string? ImageUrl { get; set; }           // placeholder for now
        public string DetailsUrl { get; set; } = "";    // link to details page
    }
}