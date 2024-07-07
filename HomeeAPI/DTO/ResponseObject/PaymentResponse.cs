namespace HomeeAPI.DTO.ResponseObject
{
    public class PaymentResponse
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string? PaymentType { get; set; }

        public decimal? Discount { get; set; }

        public int? UserId { get; set; }

        public string? Status { get; set; }
    }
}
