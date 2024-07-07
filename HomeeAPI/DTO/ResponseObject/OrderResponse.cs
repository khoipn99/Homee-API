namespace HomeeAPI.DTO.ResponseObject
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int? ChefId { get; set; }

        public string? DeliveryAddress { get; set; }

        public decimal OrderPrice { get; set; }

        public int Quantity { get; set; }

        public int? UserId { get; set; }

        public string? Status { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
