using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;
using System.Security.Claims;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// Giả lập user đã đăng nhập (ClaimsPrincipal) cho Customer — dùng trong test Checkout.
/// </summary>
public static class CustomerPrincipalFactoryForTests
{
    public static ClaimsPrincipal CreateFromCustomer(Customer customer)
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        AccountAuthService accountAuthService = new AccountAuthService(context);
        ClaimsPrincipal principal = accountAuthService.CreateCustomerPrincipal(customer);

        context.Dispose();

        return principal;
    }
}
