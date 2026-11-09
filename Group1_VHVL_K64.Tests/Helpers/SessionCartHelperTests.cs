using Group1_VHVL_K64.Helpers;
using Group1_VHVL_K64.Models;
using Xunit;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// Test lưu giỏ hàng trong Session (JSON) — không cần database.
/// </summary>
public class SessionCartHelperTests
{
    [Fact]
    public void GetCartItems_WhenSessionHasNoCartData_ThenReturnsEmptyList()
    {
        TestSession session = new TestSession();

        List<CartSessionItem> items = SessionCartHelper.GetCartItems(session);

        Assert.Empty(items);
    }

    [Fact]
    public void SaveCartItems_WhenCalled_ThenGetCartItemsReturnsSameLines()
    {
        TestSession session = new TestSession();

        List<CartSessionItem> cartToSave = new List<CartSessionItem>();
        cartToSave.Add(new CartSessionItem { ProductId = 1, Quantity = 2 });
        cartToSave.Add(new CartSessionItem { ProductId = 5, Quantity = 1 });

        SessionCartHelper.SaveCartItems(session, cartToSave);

        List<CartSessionItem> cartLoaded = SessionCartHelper.GetCartItems(session);

        Assert.Equal(2, cartLoaded.Count);

        int totalQuantity = SessionCartHelper.GetTotalQuantity(session);
        Assert.Equal(3, totalQuantity);
    }

    [Fact]
    public void ClearCart_WhenCartHadItems_ThenGetCartItemsReturnsEmpty()
    {
        TestSession session = new TestSession();

        List<CartSessionItem> cartToSave = new List<CartSessionItem>();
        cartToSave.Add(new CartSessionItem { ProductId = 1, Quantity = 1 });
        SessionCartHelper.SaveCartItems(session, cartToSave);

        SessionCartHelper.ClearCart(session);

        List<CartSessionItem> cartLoaded = SessionCartHelper.GetCartItems(session);
        Assert.Empty(cartLoaded);
    }
}
