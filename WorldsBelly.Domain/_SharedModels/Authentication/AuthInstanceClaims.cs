using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorldsBelly.Domain.Utils.Constants;

namespace WorldsBelly.Domain._SharedModels.Authentication
{
    public class AuthInstanceClaims
    {
        public string OrganizationId { get; set; }
        public string Email { get; set; }

        public AuthInstanceClaims(IEnumerable<Claim> claims)
        {
            Email = claims.SingleOrDefault(_ => _.Type == JwtTokenClaims.UserEmail)?.Value;
        }
    }
}
