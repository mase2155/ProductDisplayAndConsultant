using System;
using System.Collections.Generic;

namespace Group1_VHVL_K64.Models.DBModel;

public partial class CustomerInquiry
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
