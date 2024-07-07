﻿using System;
using System.Collections.Generic;

namespace HomeeRepositories.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
