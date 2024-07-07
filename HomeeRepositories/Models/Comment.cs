using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentDate { get; set; }

    public int? OrderId { get; set; }

    public int? Star { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
