using Microsoft.AspNetCore.Http;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.Domain.Utils.Helpers
{
    public static class HeaderHelper
    {
        public static IHeaderDictionary Headers { get; set; }
        public static string UserId { get; set; }
        public static string LanguageCode { get; set; }
        public static void SetGlobals(IHeaderDictionary header)
        {
            Headers = header;
            var token = TokenHelper.ReadToken(GetToken());
            UserId = token.Claims.First(claim => claim.Type == "uid").Value;
            LanguageCode = GetHeader("Content-Language");
        }
        public static string GetHeader(string value)
        {
           Headers.TryGetValue(value, out var content);
           return content;
        }
        public static string GetToken()
        {
            Headers.TryGetValue("Authorization", out var content);
            return content.ToString().Replace("Bearer ", "");
        }
    }
}