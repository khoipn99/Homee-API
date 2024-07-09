namespace HomeeAPI.DTO.RequestObject
{
    public class FoodRequest
    {
        public string Name { get; set; }
        public string FoodType { get; set; }
        public decimal Price { get; set; }
        public decimal SellPrice { get; set; }
        public int CategoryId { get; set; }
        public int ChefId { get; set; }
        public int SellCount { get; set; }
        public string Status { get; set; }
    }

}
