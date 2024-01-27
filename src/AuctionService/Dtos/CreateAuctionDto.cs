using System.ComponentModel.DataAnnotations;

namespace AuctionService.Dtos
{
    public class CreateAuctionDto
    {
        [Required(ErrorMessage = "Make is required")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Mileage is required")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }

        [Required(ErrorMessage = "ReservePrice is required")]
        public int ReservePrice { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "AuctionEnd is required")]
        public DateTime AuctionEnd { get; set; }
    }
}