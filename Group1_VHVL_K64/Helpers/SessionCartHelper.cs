using System.Text.Json;
using Group1_VHVL_K64.Models;

namespace Group1_VHVL_K64.Helpers
{
    public static class SessionCartHelper
    {
        private const string CartSessionKey = "CartItems";

        public static List<CartSessionItem> GetCartItems(ISession session)
        {
            string? json = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json))
            {
                return new List<CartSessionItem>();
            }

            List<CartSessionItem>? items = JsonSerializer.Deserialize<List<CartSessionItem>>(json);
            if (items == null)
            {
                return new List<CartSessionItem>();
            }

            return items;
        }

        public static void SaveCartItems(ISession session, List<CartSessionItem> items)
        {
            string json = JsonSerializer.Serialize(items);
            session.SetString(CartSessionKey, json);
        }

        public static int GetTotalQuantity(ISession session)
        {
            List<CartSessionItem> items = GetCartItems(session);
            int total = 0;

            foreach (CartSessionItem item in items)
            {
                total += item.Quantity;
            }

            return total;
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }
    }
}
