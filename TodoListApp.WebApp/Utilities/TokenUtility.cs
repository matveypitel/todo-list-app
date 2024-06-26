using System.Net.Http.Headers;

namespace TodoListApp.WebApp.Utilities;

/// <summary>
/// Utility class for handling tokens.
/// </summary>
public static class TokenUtility
{
    /// <summary>
    /// Gets the token from the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The token value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public static string GetToken(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Cookies.TryGetValue("Token", out var token))
        {
            return token!;
        }

        return string.Empty;
    }

    /// <summary>
    /// Adds the authorization header to the HTTP client.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="token">The token value.</param>
    /// <exception cref="ArgumentNullException">Thrown when the httpClient is null.</exception>
    public static void AddAuthorizationHeader(HttpClient httpClient, string token)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
