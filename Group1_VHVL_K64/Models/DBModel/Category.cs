using System;
using System.Collections.Generic;

namespace Group1_VHVL_K64.Models.DBModel;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
