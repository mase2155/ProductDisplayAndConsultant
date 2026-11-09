using Group1_VHVL_K64.Helpers;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace Group1_VHVL_K64.Tests.Services;

/// <summary>
/// Test thanh toán giỏ hàng — mỗi case mô phỏng một tình huống khách gặp trên web.
/// </summary>
public class CheckoutServiceTests
{
    [Fact]
    public async Task PlaceOrderFromCartAsync_WhenUserIsGuest_ThenNeedsLogin()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        CheckoutService checkoutService = CheckoutServiceFactoryForTests.Create(context);
        TestSession session = new TestSession();
        ClaimsPrincipal guestUser = new ClaimsPrincipal();

        CheckoutResult result = await checkoutService.PlaceOrderFromCartAsync(guestUser, session);

        Assert.False(result.Success);
        Assert.True(result.NeedLogin);
    }

    [Fact]
    public async Task PlaceOrderFromCartAsync_WhenCustomerHasNoPhoneOrAddress_ThenNeedsProfileUpdate()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        Customer customer = await TestDbContextFactory.SeedCustomerAsync(
            context,
            "c@test.com",
            SampleTestData.StrongPassword);

        ClaimsPrincipal loggedInCustomer = CustomerPrincipalFactoryForTests.CreateFromCustomer(customer);

        CheckoutService checkoutService = CheckoutServiceFactoryForTests.Create(context);
        TestSession session = new TestSession();

        List<CartSessionItem> cartItems = new List<CartSessionItem>();
        cartItems.Add(new CartSessionItem { ProductId = 1, Quantity = 1 });
        SessionCartHelper.SaveCartItems(session, cartItems);

        CheckoutResult result = await checkoutService.PlaceOrderFromCartAsync(loggedInCustomer, session);

        Assert.False(result.Success);
        Assert.True(result.NeedProfileUpdate);
    }

    [Fact]
    public async Task PlaceOrderFromCartAsync_WhenCartIsEmpty_ThenFailsWithMessage()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        Customer customer = await TestDbContextFactory.SeedCustomerAsync(
            context,
            "c@test.com",
            SampleTestData.StrongPassword,
            phone: SampleTestData.ValidPhone,
            address: SampleTestData.ValidShippingAddress);

        ClaimsPrincipal loggedInCustomer = CustomerPrincipalFactoryForTests.CreateFromCustomer(customer);

        CheckoutService checkoutService = CheckoutServiceFactoryForTests.Create(context);
        TestSession session = new TestSession();

        CheckoutResult result = await checkoutService.PlaceOrderFromCartAsync(loggedInCustomer, session);

        Assert.False(result.Success);

        bool messageSaysEmpty = result.Message.Contains("trống", StringComparison.OrdinalIgnoreCase);
        Assert.True(messageSaysEmpty);
    }

    [Fact]
    public async Task PlaceOrderFromCartAsync_WhenCartIsValid_ThenCreatesOrderAndClearsSession()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        await TestDbContextFactory.SeedBasicCatalogAsync(context);

        Product productInDatabase = await context.Products.FirstAsync();

        Customer customer = await TestDbContextFactory.SeedCustomerAsync(
            context,
            "buyer@test.com",
            SampleTestData.StrongPassword,
            phone: SampleTestData.ValidPhone,
            address: SampleTestData.ValidShippingAddress);

        ClaimsPrincipal loggedInCustomer = CustomerPrincipalFactoryForTests.CreateFromCustomer(customer);

        CheckoutService checkoutService = CheckoutServiceFactoryForTests.Create(context);
        TestSession session = new TestSession();

        List<CartSessionItem> cartItems = new List<CartSessionItem>();
        cartItems.Add(new CartSessionItem { ProductId = productInDatabase.Id, Quantity = 2 });
        SessionCartHelper.SaveCartItems(session, cartItems);

        CheckoutResult result = await checkoutService.PlaceOrderFromCartAsync(loggedInCustomer, session);

        Assert.True(result.Success);
        Assert.NotNull(result.OrderId);

        List<CartSessionItem> cartAfterCheckout = SessionCartHelper.GetCartItems(session);
        Assert.Empty(cartAfterCheckout);
    }
}
