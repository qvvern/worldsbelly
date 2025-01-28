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
    //public static class GlobalHeader
    //{
    //    public static Guid UserId { get; set; }
    //    public static int LanguageId { get; set; }
    //    public static void SetUserId(IHeaderDictionary header)
    //    {
    //        header.TryGetValue("Authorization", out var headerToken);
    //        var tokenValue = headerToken.ToString().Replace("Bearer ", "");
    //        var handler = new JwtSecurityTokenHandler();
    //        var token = handler.ReadToken(tokenValue) as JwtSecurityToken;
    //        UserId = Guid.Parse(token.Claims.First(claim => claim.Type == "oid").Value);
    //    }
    //    public static void SetLanguageId(IHeaderDictionary header)
    //    {
    //        header.TryGetValue("Language-Id", out var headerLanguageId);
    //        LanguageId = int.Parse(headerLanguageId.ToString());
    //    }
    //}
}