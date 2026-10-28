using System;
using System.Collections.Generic;

namespace Group1_VHVL_K64.Models.DBModel;

public partial class Admin
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PlainPassword { get; set; } = null!;

    public string? PasswordHash { get; set; }
}
