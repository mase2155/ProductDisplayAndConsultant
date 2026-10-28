using System;
using System.Collections.Generic;

namespace Group1_VHVL_K64.Models.DBModel;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public decimal? RateByStars { get; set; }

    public decimal Price { get; set; }

    public decimal? NewPrice { get; set; }

    public string? Urlimage { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
