using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? ChefId { get; set; }

    public string? DeliveryAddress { get; set; }

    public decimal OrderPrice { get; set; }

    public int Quantity { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual Chef? Chef { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
