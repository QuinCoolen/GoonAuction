using System;
using System.ComponentModel.DataAnnotations;

namespace GoonAuctionBLL.Dto {
  public class CreateEditAuctionDto {
    
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Starting price is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Starting price must be greater than 0.")]
    public int StartingPrice { get; set; }
    [Required(ErrorMessage = "Increment is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Increment must be greater than 0.")]
    public int CurrentPrice { get; set; }
    [Required(ErrorMessage = "Increment is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Increment must be greater than 0.")]
    public int Increment { get; set; }
    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(AuctionStatusDto), ErrorMessage = "Invalid status.")]
    public AuctionStatusDto Status { get; set; }
    [Required(ErrorMessage = "Image URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string ImageUrl { get; set; }
    [Required(ErrorMessage = "End date is required.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "User ID is required.")]
    [StringLength(450, ErrorMessage = "User ID cannot be longer than 450 characters.")]
    public string UserId { get; set; }
  }
}