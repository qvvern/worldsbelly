using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.Domain.Utils.Helpers
{
    public static class TokenHelper
    {
        public static JwtSecurityToken ReadToken(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(authToken) as JwtSecurityToken;
        }
    }
}
