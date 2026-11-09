using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Group1_VHVL_K64.Tests.Integration;

/// <summary>
/// Gửi GET tới các trang công khai — không cần đăng nhập.
/// </summary>
public class PublicRouteSmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public PublicRouteSmokeTests(CustomWebApplicationFactory factory)
    {
        WebApplicationFactoryClientOptions clientOptions = new WebApplicationFactoryClientOptions();
        clientOptions.AllowAutoRedirect = false;

        _httpClient = factory.CreateClient(clientOptions);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    [InlineData("/Home/Privacy")]
    [InlineData("/Home/Partner")]
    [InlineData("/Product")]
    [InlineData("/Cart")]
    [InlineData("/Contact")]
    [InlineData("/News")]
    [InlineData("/Account/Login")]
    [InlineData("/Account/Register")]
    [InlineData("/Chatbot/Frame")]
    public async Task GetPublicPage_WhenNotLoggedIn_ThenReturnsOkOrRedirect(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        HttpStatusCode statusCode = response.StatusCode;

        bool isAcceptable = HttpStatusAssertionHelper.IsSuccessOrRedirect(statusCode);

        Assert.True(
            isAcceptable,
            "GET " + url + " tra ve ma " + (int)statusCode + " (" + statusCode + ")");
    }
}
