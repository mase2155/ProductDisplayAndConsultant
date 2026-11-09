using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Group1_VHVL_K64.Tests.Integration;

/// <summary>
/// Trang Admin phải redirect về login nếu chưa đăng nhập.
/// </summary>
public class AdminRouteSmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public AdminRouteSmokeTests(CustomWebApplicationFactory factory)
    {
        WebApplicationFactoryClientOptions clientOptions = new WebApplicationFactoryClientOptions();
        clientOptions.AllowAutoRedirect = false;

        _httpClient = factory.CreateClient(clientOptions);
    }

    [Theory]
    [InlineData("/Admins/Dashboard")]
    [InlineData("/Admins/Products")]
    [InlineData("/Admins/Orders")]
    [InlineData("/Admins/Reports/DailyRevenue")]
    public async Task GetAdminPage_WhenNotLoggedIn_ThenRedirectsToLogin(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        HttpStatusCode statusCode = response.StatusCode;

        bool isRedirect = HttpStatusAssertionHelper.IsRedirect(statusCode);

        Assert.True(
            isRedirect,
            "GET " + url + " phai redirect khi chua login, ma hien tai: " + (int)statusCode);
    }
}
