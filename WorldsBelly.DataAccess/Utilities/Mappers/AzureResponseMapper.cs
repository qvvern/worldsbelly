using System.Linq;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Utilities.Mappers
{
    public static class AzureResponseMapper
    {
        public static AzureAdB2CUser Map(AzureAdB2CUserExtension azureAdB2CUserExtension)
        {
            if (azureAdB2CUserExtension == null)
            {
                return null;
            }

            return new AzureAdB2CUser()
            {
                Id = azureAdB2CUserExtension.Id,
                GivenName = azureAdB2CUserExtension.GivenName,
                Surname = azureAdB2CUserExtension.Surname,
                Mail = azureAdB2CUserExtension.Identities.FirstOrDefault(_ => _.SignInType == "emailAddress")?.IssuerAssignedId ?? azureAdB2CUserExtension.otherMails.FirstOrDefault(),
                VerificationCode = azureAdB2CUserExtension.Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCode,
                VerificationCodeExpiration = azureAdB2CUserExtension.Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCodeExpiration,
                VerificationNewEmail = azureAdB2CUserExtension.Extension_3292f8b5e07143359265da3e8e35bdbd_VerificationNewEmail,
                DeleteUserVerificationCode = azureAdB2CUserExtension.Extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCode,
                DeleteUserVerificationCodeExpiration = azureAdB2CUserExtension.Extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCodeExpiration
            };
        }
        public static AzureAdB2CUserSimple MapSimple(AzureAdB2CUserExtension azureAdB2CUserExtension)
        {
            if (azureAdB2CUserExtension == null)
            {
                return null;
            }

            return new AzureAdB2CUserSimple()
            {
                Id = azureAdB2CUserExtension.Id,
                GivenName = azureAdB2CUserExtension.GivenName,
                Surname = azureAdB2CUserExtension.Surname,
                Mail = azureAdB2CUserExtension.Identities.FirstOrDefault(_ => _.SignInType == "emailAddress")?.IssuerAssignedId,
            };
        }
    }
}
