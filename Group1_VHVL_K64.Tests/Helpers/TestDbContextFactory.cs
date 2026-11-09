using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Tests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// Tạo database InMemory trong RAM — mỗi test nên dùng tên database khác nhau (Guid).
/// </summary>
public static class TestDbContextFactory
{
    public static HoangHoanFurnitureContext Create(string uniqueDatabaseName)
    {
        DbContextOptionsBuilder<HoangHoanFurnitureContext> optionsBuilder =
            new DbContextOptionsBuilder<HoangHoanFurnitureContext>();

        optionsBuilder.UseInMemoryDatabase(uniqueDatabaseName);

        optionsBuilder.ConfigureWarnings(warning =>
        {
            warning.Ignore(InMemoryEventId.TransactionIgnoredWarning);
        });

        DbContextOptions<HoangHoanFurnitureContext> options = optionsBuilder.Options;

        HoangHoanFurnitureContext context = new HoangHoanFurnitureContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    public static string CreateUniqueDatabaseName()
    {
        return "TestDb_" + Guid.NewGuid().ToString("N");
    }

    public static async Task SeedBasicCatalogAsync(HoangHoanFurnitureContext context)
    {
        Category category = new Category();
        category.Name = "Danh muc test";

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        Product product = new Product();
        product.Name = "Ghe test";
        product.Price = 100000m;
        product.NewPrice = 90000m;
        product.CategoryId = category.Id;

        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public static async Task<Customer> SeedCustomerAsync(
        HoangHoanFurnitureContext context,
        string email,
        string password,
        string? phone = null,
        string? address = null)
    {
        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        string passwordHash = passwordHasher.HashPassword(null!, password);

        Customer customer = new Customer();
        customer.Email = email;
        customer.Fullname = SampleTestData.ValidFullName;
        customer.PlainPassword = password;
        customer.PasswordHash = passwordHash;
        customer.Phone = phone;
        customer.ShippingAddress = address;

        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        return customer;
    }

    public static async Task<Admin> SeedAdminAsync(
        HoangHoanFurnitureContext context,
        string email,
        string password)
    {
        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        string passwordHash = passwordHasher.HashPassword(null!, password);

        Admin admin = new Admin();
        admin.Email = email;
        admin.Fullname = "Admin Test";
        admin.PlainPassword = password;
        admin.PasswordHash = passwordHash;

        context.Admins.Add(admin);
        await context.SaveChangesAsync();

        return admin;
    }
}
