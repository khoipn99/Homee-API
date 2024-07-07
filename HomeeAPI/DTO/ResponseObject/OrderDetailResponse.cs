namespace HomeeAPI.DTO.ResponseObject
{
    public class OrderDetailResponse
    {
        public int Id { get; set; }

        public int? FoodId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int? OrderId { get; set; }

        public string? Status { get; set; }
    }
}
