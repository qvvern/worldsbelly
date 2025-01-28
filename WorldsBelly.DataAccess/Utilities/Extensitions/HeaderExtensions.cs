using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class HeaderExtensions
    {
        public static Guid GetAzureId(this IHeaderDictionary header)
        {
            header.TryGetValue("Authorization", out var headerToken);
            var tokenValue = headerToken.ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(tokenValue) as JwtSecurityToken;
            return Guid.Parse(token.Claims.First(claim => claim.Type == "oid").Value?.ToString());
        }
        public static int GetLanguageId(this IHeaderDictionary header)
        {
            header.TryGetValue("Language-Id", out var headerLanguageId);
            return int.Parse(headerLanguageId.ToString());
        }
    }
}
