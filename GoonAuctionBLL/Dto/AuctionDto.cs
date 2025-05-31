using System;
using System.ComponentModel.DataAnnotations;

namespace GoonAuctionBLL.Dto {
  public class AuctionDto {
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Starting price is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Starting price must be greater than 0.")]
    [DataType(DataType.Currency)]
    public int StartingPrice { get; set; }
    public int CurrentPrice { get; set; }
    public int Increment { get; set; }
    public string Status { get; set; }
    public string ImageUrl { get; set; }
    [Required(ErrorMessage = "End date is required.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Range(typeof(DateTime), "1/1/2025", "12/31/2026", ErrorMessage = "End date must be between 1/1/2025 and 12/31/2026.")]
    public UserDto User { get; set; }
    public DateTime EndDate { get; set; }
  }
}