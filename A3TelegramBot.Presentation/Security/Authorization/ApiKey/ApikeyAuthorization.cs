using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace A3TelegramBot.Presentation.Security.Authorization.ApiKey;

internal sealed class ApikeyAuthorization(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<ApiKeyAuthOptions> authOptions)
    :AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    internal const string SchemeName = "ApiKey";
    internal const string HeaderName = "x-api-key";

    private readonly string _apiKey = authOptions.Value.ApiKey ?? throw new InvalidOperationException("Api key not configured");

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(HeaderName, out var extractedApiKey);

        if (!IsPublicEndpoint() && !extractedApiKey.Equals(_apiKey))
            return Task.FromResult(AuthenticateResult.Fail("Invalid API credentials!"));

        var identity = new ClaimsIdentity(
                                          [new("ClientID", "Default")],
                                          Scheme.Name);

        var principal = new GenericPrincipal(identity, null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private bool IsPublicEndpoint() => Context.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is null or true;
}