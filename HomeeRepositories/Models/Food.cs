using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Food
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? FoodType { get; set; }

    public decimal Price { get; set; }

    public decimal SellPrice { get; set; }

    public int? CategoryId { get; set; }

    public int? ChefId { get; set; }

    public int? SellCount { get; set; }

    public string? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Chef? Chef { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
