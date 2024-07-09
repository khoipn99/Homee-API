namespace HomeeAPI.DTO.RequestObject
{
    public class ChefRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int? CreatorId { get; set; }
        public decimal? Score { get; set; }
        public int? Hours { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public decimal? Money { get; set; }
        public string Banking { get; set; }
    }
}
