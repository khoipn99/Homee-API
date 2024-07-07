namespace HomeeAPI.DTO.ResponseObject
{
    public class VoucherResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal? Discount { get; set; }

        public int? Quantity { get; set; }
    }

}
