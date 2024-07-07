namespace HomeeAPI.DTO.ResponseObject
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Avatar { get; set; }

        public int? RoleId { get; set; }

        public string? Status { get; set; }

        public decimal? Money { get; set; }

        public decimal? Discount { get; set; }
    }
}
