using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class AzureAdB2CUserSimple
    {
        public string GivenName { get; set; }
        public string Mail { get; set; }
        public string Surname { get; set; }
        public Guid Id { get; set; }
    }
    public class AzureAdB2CUser
    {
        public string GivenName { get; set; }
        public string Mail { get; set; }
        public string Surname { get; set; }
        public string VerificationCode { get; set; }
        public string VerificationCodeExpiration { get; set; }
        public string VerificationNewEmail { get; set; }
        public string DeleteUserVerificationCode { get; set; }
        public string DeleteUserVerificationCodeExpiration { get; set; }
        public Guid Id { get; set; }
    }
    public class AzureAdB2CUserExtension
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public List<string> otherMails { get; set; }
        public string Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCode { get; set; }
        public string Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCodeExpiration { get; set; }
        public string Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationNewEmail { get; set; }
        public string Extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCode { get; set; }
        public string Extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCodeExpiration { get; set; }
        public List<AzureAdB2CUserDetailedIdentity> Identities { get; set; }
        public Guid Id { get; set; }
    }
    public class AzureAdB2CUserDetailedIdentity
    {
        public string SignInType { get; set; }
        public string Issuer { get; set; }
        public string IssuerAssignedId { get; set; }
    }
}
