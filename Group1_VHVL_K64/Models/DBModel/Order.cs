namespace Group1_VHVL_K64.Models.DBModel;

public partial class Order
{
    public int Id { get; set; }

    /// <summary>Số thứ tự đơn hiển thị cho người dùng (1, 2, 3...), không nhảy cóc khi Id identity nhảy.</summary>
    public int OrderNumber { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = "Pending";

    public decimal TotalAmount { get; set; }

    public string? ShippingAddress { get; set; }

    public string? Phone { get; set; }

    public string? Note { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
