using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Chef
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public int? CreatorId { get; set; }

    public string? ProfilePicture { get; set; }

    public decimal? Score { get; set; }

    public int? Hours { get; set; }

    public string? Status { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public decimal? Money { get; set; }

    public string? Banking { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TopUpRequest> TopUpRequests { get; set; } = new List<TopUpRequest>();
}
