using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Group1_VHVL_K64.Tests.Integration;

/// <summary>
/// Khởi chạy web app thật trong test nhưng thay SQL bằng InMemory.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Bước 1: Gỡ đăng ký DbContext cũ (SQL Server từ appsettings)
            ServiceDescriptor? oldDbContextRegistration = null;

            foreach (ServiceDescriptor service in services)
            {
                if (service.ServiceType == typeof(DbContextOptions<HoangHoanFurnitureContext>))
                {
                    oldDbContextRegistration = service;
                    break;
                }
            }

            if (oldDbContextRegistration != null)
            {
                services.Remove(oldDbContextRegistration);
            }

            // Bước 2: Đăng ký DbContext InMemory
            string inMemoryDatabaseName = "IntegrationTests_" + Guid.NewGuid().ToString("N");

            services.AddDbContext<HoangHoanFurnitureContext>(options =>
            {
                options.UseInMemoryDatabase(inMemoryDatabaseName);
                options.ConfigureWarnings(warning =>
                {
                    warning.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
            });
        });
    }
}
