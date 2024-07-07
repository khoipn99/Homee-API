using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class TopUpRequest
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ChefId { get; set; }

    public decimal Amount { get; set; }

    public DateTime RequestDate { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public virtual Chef? Chef { get; set; }

    public virtual User? User { get; set; }
}
