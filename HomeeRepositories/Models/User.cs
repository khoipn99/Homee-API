using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? Avatar { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public decimal? Money { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TopUpRequest> TopUpRequests { get; set; } = new List<TopUpRequest>();
}
