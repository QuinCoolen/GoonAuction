namespace GoonAuctionBLL.Dto
{
    public class EditStatusAuctionDto
    {
        public int Id { get; set; }
        public string Status { get; set; } // e.g., "Active", "Completed", "Cancelled"
    }
}