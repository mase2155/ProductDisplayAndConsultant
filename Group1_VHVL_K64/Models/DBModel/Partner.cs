using System;
using System.Collections.Generic;

namespace Group1_VHVL_K64.Models.DBModel;

public partial class Partner
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? LogoUrl { get; set; }

    public string? WebsiteUrl { get; set; }
}
