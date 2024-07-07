namespace HomeeAPI.DTO.ResponseObject
{
    public class TopUpRequestResponse
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ChefId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
