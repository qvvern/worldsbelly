using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Mappers;
using WorldsBelly.Domain.Models.Authentication;

namespace WorldsBelly.DataAccess.Services
{
    public class AzureAdB2CService : IAzureAdB2CService
    {
        private readonly IAzureGraphEmailService _azureGraphEmailService;
        private const string SecretKey = "";
        private const string ClientId = "";
        private const string TenantId = "";

        private static readonly IRestClient RestClient = new RestClient("https://login.microsoftonline.com/");
        private static readonly IRestClient RestClientGraph = new RestClient("https://graph.microsoft.com/");

        public AzureAdB2CService(IAzureGraphEmailService azureGraphEmailService)
        {
            _azureGraphEmailService = azureGraphEmailService;
        }

        private async Task<AuthInstance> AuthenticateAsync()
        {
            RestRequest request = new RestRequest($"{TenantId}/oauth2/token", Method.POST);
            request.AddParameter("client_id", ClientId, ParameterType.GetOrPost);
            request.AddParameter("client_secret", SecretKey, ParameterType.GetOrPost);
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);
            request.AddParameter("resource", "https://graph.microsoft.com", ParameterType.GetOrPost);

            var response = await RestClient.ExecuteAsync<AuthInstance>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not authenticate with B2C");
            }
            return JsonConvert.DeserializeObject<AuthInstance>(response.Content);
        }

        public async Task<List<AzureAdB2CUser>> GetUsersAsync()
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users", Method.GET);
            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not authenticate token");
            }
            return JsonConvert.DeserializeObject<List<AzureAdB2CUser>>(response.Content);
        }
        public async Task<AzureAdB2CUser> GetUserAsync(string id)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}?$select=id,otherMails,surName,givenName,identities,extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCode,extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCodeExpiration,extension_3292f8b5e07143359265da3e8e35bdbd_VerificationNewEmail,extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCode,extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCodeExpiration", Method.GET);
            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not authenticate token");
            }
            var azureAdB2CUserExtension = JsonConvert.DeserializeObject<AzureAdB2CUserExtension>(response.Content);
            return AzureResponseMapper.Map(azureAdB2CUserExtension);
        }
        public async Task<bool> DoesEmailAlreadyExistAsync(string email)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"beta/users?$filter=identities/any(id:id/issuer eq 'worldsbelly.onmicrosoft.com' and id/issuerAssignedId eq '{email}')", Method.GET);
            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            try
            {
                var response = await RestClientGraph.ExecuteAsync<object>(request);
                var responseData = JObject.FromObject(response.Data);
                var value = (JArray)responseData["value"];
                if (value.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Could not verify email");
            }
        }

        public async Task UpdateUserEmail(string id, string newEmail)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.PATCH);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);


            List<object> identities = new List<object>();
            identities.Add(new Dictionary<string, dynamic>
                {
                    { "SignInType", "emailAddress" },
                    { "Issuer", "worldsbelly.onmicrosoft.com" },
                    { "IssuerAssignedId", newEmail }
                });
            request.AddJsonBody(new
            {
                identities = identities
            });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not update email");
            }
            return;
        }

        public async Task DeleteUser(string id)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.DELETE);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not delete user, please contact support.");
            }
            return;
        }
        public async Task UpdateUserVerificationCode(string id, string oldEmail, string newEmail, string userToken)
        {
            if (String.IsNullOrWhiteSpace(userToken)) throw new Exception("Could not validate user.");
            if (await DoesEmailAlreadyExistAsync(newEmail))
            {
                throw new Exception("Email already exist");
            }
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.PATCH);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            var code = GenerateSecurityCode();
            DateTime currentTime = DateTime.Now;
            DateTime x30MinsLater = currentTime.AddMinutes(30);

            request.AddJsonBody(new Dictionary<string, dynamic>
                {
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCode", code },
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationNewEmail", newEmail },
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCodeExpiration", x30MinsLater.ToString() },
                });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not update email");
            }
            else
            {
                await _azureGraphEmailService.SendUserVerificationCodeEmail(oldEmail, newEmail, code);
            }
            return;
        }
        public async Task UpdateDeleteUserVerificationCode(string id, string userEmail, string userToken)
        {
            if (String.IsNullOrWhiteSpace(userToken)) throw new Exception("Could not validate user.");
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.PATCH);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            var code = GenerateSecurityCode();
            DateTime currentTime = DateTime.Now;
            DateTime x30MinsLater = currentTime.AddMinutes(30);

            request.AddJsonBody(new Dictionary<string, dynamic>
                {
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCode", code },
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_DeleteUserVerificationCodeExpiration", x30MinsLater.ToString() },
                });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not update email");
            }
            else
            {
                await _azureGraphEmailService.SendDeleteUserVerificationCodeEmail(userEmail, code);
            }
            return;
        }
        private string GenerateSecurityCode()
        {
            var randomNumber = new byte[8];
            string refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            return refreshToken;
        }
        public async Task ResetUserVerificationCode(string id)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.PATCH);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            request.AddJsonBody(new Dictionary<string, dynamic>
                {
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCode", null },
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationNewEmail", null },
                    { "extension_3292f8b5e07143359265da3e8e35bdbd_VerificationCodeExpiration", null },
                });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not update user");
            }
            return;
        }

        public async Task UpdateUserAsync(AzureAdB2CUser user, Guid id)
        {
            if (user.GivenName.Length <= 2 || user.Surname.Length <= 2)
            {
                throw new Exception("Could not update user. Length needs to be more than 2");
            }

            var token = await AuthenticateAsync().ConfigureAwait(false);

            RestRequest request = new RestRequest($"v1.0/users/{id}", Method.PATCH);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);

            request.AddJsonBody(new Dictionary<string, dynamic>
                {
                    { "GivenName", user.GivenName },
                    { "Surname", user.Surname },
                });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Could not update user");
            }
            return;

        }
    }
}
