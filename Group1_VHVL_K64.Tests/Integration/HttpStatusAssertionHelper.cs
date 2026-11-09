using System.Net;

namespace Group1_VHVL_K64.Tests.Integration;

/// <summary>
/// Kiểm tra mã HTTP — viết tách ra để test đọc dễ hơn.
/// </summary>
public static class HttpStatusAssertionHelper
{
    public static bool IsSuccessOrRedirect(HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.OK)
        {
            return true;
        }

        if (statusCode == HttpStatusCode.Redirect)
        {
            return true;
        }

        if (statusCode == HttpStatusCode.Found)
        {
            return true;
        }

        return false;
    }

    public static bool IsRedirect(HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Redirect)
        {
            return true;
        }

        if (statusCode == HttpStatusCode.Found)
        {
            return true;
        }

        return false;
    }
}
