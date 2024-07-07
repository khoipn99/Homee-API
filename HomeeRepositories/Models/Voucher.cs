using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Voucher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Discount { get; set; }

    public int Quantity { get; set; }
}
