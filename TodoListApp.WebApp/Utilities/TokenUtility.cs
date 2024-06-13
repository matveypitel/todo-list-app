using System.Net.Http.Headers;

namespace TodoListApp.WebApp.Utilities;

public static class TokenUtility
{
    public static string GetToken(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Cookies.TryGetValue("Token", out var token))
        {
            return token!;
        }

        return string.Empty;
    }

    public static void AddAuthorizationHeader(HttpClient httpClient, string token)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
