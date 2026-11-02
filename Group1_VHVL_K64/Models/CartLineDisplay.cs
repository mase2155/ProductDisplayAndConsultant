namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Dữ liệu một dòng trên trang giỏ hàng (đã gộp thông tin sản phẩm + số lượng).
    /// </summary>
    public class CartLineDisplay
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Urlimage { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal
        {
            get { return UnitPrice * Quantity; }
        }
    }
}
