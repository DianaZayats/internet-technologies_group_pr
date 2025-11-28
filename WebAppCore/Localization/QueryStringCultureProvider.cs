using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebAppCore.Localization
{
    /// <summary>
    /// Постачальник культури, що читає значення з query string (?culture=xx-YY) та оновлює cookie.
    /// </summary>
    public class QueryStringCultureProvider : RequestCultureProvider
    {
        private readonly Dictionary<string, string> _supportedCultures;
        private static readonly CookieOptions CookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            Path = "/",
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        };

        public QueryStringCultureProvider(IEnumerable<string> supportedCultures)
        {
            _supportedCultures = supportedCultures
                .ToDictionary(c => c.ToLowerInvariant(), c => c, StringComparer.OrdinalIgnoreCase);
        }

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var cultureQuery = httpContext.Request.Query["culture"].ToString();

            if (string.IsNullOrWhiteSpace(cultureQuery))
            {
                return Task.FromResult<ProviderCultureResult?>(null);
            }

            var normalized = cultureQuery.ToLowerInvariant();
            if (!_supportedCultures.TryGetValue(normalized, out var canonicalCulture))
            {
                return Task.FromResult<ProviderCultureResult?>(null);
            }

            var cultureInfo = new CultureInfo(canonicalCulture);
            var requestCulture = new RequestCulture(cultureInfo);

            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(requestCulture),
                CookieOptions);

            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(canonicalCulture, canonicalCulture));
        }
    }
}

