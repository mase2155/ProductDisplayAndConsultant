namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Phần tử lưu trong Session (chỉ id sản phẩm và số lượng).
    /// </summary>
    public class CartSessionItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
