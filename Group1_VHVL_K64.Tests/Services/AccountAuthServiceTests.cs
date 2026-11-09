using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Services;

/// <summary>
/// Test đăng nhập / đăng ký — luồng: tạo DB test → seed user → gọi AccountAuthService.
/// </summary>
public class AccountAuthServiceTests
{
    [Fact]
    public async Task LoginAsync_WhenCustomerPasswordIsCorrect_ThenReturnsSuccessAndUserRole()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        string customerEmail = "cust@test.com";
        await TestDbContextFactory.SeedCustomerAsync(context, customerEmail, SampleTestData.StrongPassword);

        AccountAuthService accountAuthService = new AccountAuthService(context);

        AccountAuthResult loginResult = await accountAuthService.LoginAsync(
            customerEmail,
            SampleTestData.StrongPassword);

        Assert.True(loginResult.Success);
        Assert.NotNull(loginResult.Principal);
        Assert.True(loginResult.Principal.IsInRole(AppRoles.User));
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ThenReturnsFailure()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        string customerEmail = "cust@test.com";
        await TestDbContextFactory.SeedCustomerAsync(context, customerEmail, SampleTestData.StrongPassword);

        AccountAuthService accountAuthService = new AccountAuthService(context);

        AccountAuthResult loginResult = await accountAuthService.LoginAsync(customerEmail, "WrongPass1!");

        Assert.False(loginResult.Success);

        bool messageContainsHint = loginResult.ErrorMessage.Contains(
            "không đúng",
            StringComparison.OrdinalIgnoreCase);

        Assert.True(messageContainsHint);
    }

    [Fact]
    public async Task LoginAsync_WhenAdminPasswordIsCorrect_ThenReturnsSuccessAndAdminRole()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        string adminEmail = "admin@test.com";
        string adminPassword = "AdminPass1!";
        await TestDbContextFactory.SeedAdminAsync(context, adminEmail, adminPassword);

        AccountAuthService accountAuthService = new AccountAuthService(context);

        AccountAuthResult loginResult = await accountAuthService.LoginAsync(adminEmail, adminPassword);

        Assert.True(loginResult.Success);
        Assert.NotNull(loginResult.Principal);
        Assert.True(loginResult.Principal.IsInRole(AppRoles.Admin));
    }

    [Fact]
    public async Task RegisterCustomerAsync_WhenEmailAlreadyExists_ThenReturnsFailure()
    {
        string databaseName = TestDbContextFactory.CreateUniqueDatabaseName();
        await using HoangHoanFurnitureContext context = TestDbContextFactory.Create(databaseName);

        string email = "dup@test.com";
        await TestDbContextFactory.SeedCustomerAsync(context, email, SampleTestData.StrongPassword);

        AccountAuthService accountAuthService = new AccountAuthService(context);

        CustomerRegistrationResult registerResult = await accountAuthService.RegisterCustomerAsync(
            email,
            "Nguyen Van B",
            SampleTestData.StrongPassword);

        Assert.False(registerResult.Success);
    }
}
