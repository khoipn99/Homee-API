using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? FoodId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int? OrderId { get; set; }

    public string? Status { get; set; }

    public virtual Food? Food { get; set; }

    public virtual Order? Order { get; set; }
}
