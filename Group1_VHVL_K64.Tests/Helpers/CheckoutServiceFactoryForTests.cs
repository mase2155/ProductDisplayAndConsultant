using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// Tạo CheckoutService với đủ dependency như Program.cs (để test không phải gọi DI phức tạp).
/// </summary>
public static class CheckoutServiceFactoryForTests
{
    public static CheckoutService Create(HoangHoanFurnitureContext context)
    {
        AccountAuthService accountAuthService = new AccountAuthService(context);
        CustomerProfileService customerProfileService = new CustomerProfileService(context, accountAuthService);
        OrderPersistenceService orderPersistenceService = new OrderPersistenceService(context);

        CheckoutService checkoutService = new CheckoutService(
            context,
            customerProfileService,
            orderPersistenceService);

        return checkoutService;
    }
}
