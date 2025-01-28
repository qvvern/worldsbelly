using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.Domain.Models.Authentication;

namespace WorldsBelly.DataAccess.Services
{
    public class AzureGraphEmailService : IAzureGraphEmailService
    {

        private const string SecretKey = "";
        private const string ClientId = "";
        private const string TenantId = "";

        private static readonly IRestClient RestClient = new RestClient("https://login.microsoftonline.com/");
        private static readonly IRestClient RestClientGraph = new RestClient("https://graph.microsoft.com/");
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

        public async Task SendUserVerificationCodeEmail(string oldEmail, string newEmail, string verificationCode)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);
            RestRequest request = new RestRequest($"v1.0/users/bf44e2cc-c518-42d9-baf4-4cfe1221cb90/sendMail", Method.POST);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);
            var toRecipients = new List<Dictionary<string, dynamic>>();
            toRecipients.Add(new Dictionary<string, dynamic>
                {
                    { "emailAddress", new Dictionary<string, dynamic>
                            {
                                { "address", $"{oldEmail}" },
                            }
                    }
                });
            request.AddJsonBody(new
            {
                message = new Dictionary<string, dynamic>
                {
                    { "subject", "Worldsbelly, change email verification code." },
                    { "body", new Dictionary<string, dynamic>
                            {
                                { "contentType", "Text" },
                                { "content", $"You are about to change the email of your worldsbelly's account to: {newEmail}. \nPlease use verification code: \n\n{verificationCode}" }
                            }
                    },
                    { "toRecipients", toRecipients }
                },
                saveToSentItems = "false",
            });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Could not send email");
            }
            return;
        }

        public async Task SendDeleteUserVerificationCodeEmail(string email, string verificationCode)
        {
            var token = await AuthenticateAsync().ConfigureAwait(false);
            RestRequest request = new RestRequest($"v1.0/users/bf44e2cc-c518-42d9-baf4-4cfe1221cb90/sendMail", Method.POST);

            request.AddParameter("Authorization", $"Bearer {token.AccessToken}", ParameterType.HttpHeader);
            var toRecipients = new List<Dictionary<string, dynamic>>();
            toRecipients.Add(new Dictionary<string, dynamic>
                {
                    { "emailAddress", new Dictionary<string, dynamic>
                            {
                                { "address", $"{email}" },
                            }
                    }
                });
            request.AddJsonBody(new
            {
                message = new Dictionary<string, dynamic>
                {
                    { "subject", "Worldsbelly, delete account." },
                    { "body", new Dictionary<string, dynamic>
                            {
                                { "contentType", "Text" },
                                { "content", $"You are about to delete your worldsbelly's account. \nPlease use verification code: \n\n{verificationCode}" }
                            }
                    },
                    { "toRecipients", toRecipients }
                },
                saveToSentItems = "false",
            });

            var response = await RestClientGraph.ExecuteAsync<object>(request);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Could not send email");
            }
            return;
        }
    }
}
