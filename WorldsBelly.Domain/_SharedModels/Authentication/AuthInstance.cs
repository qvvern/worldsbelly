using Newtonsoft.Json;

namespace WorldsBelly.Domain.Models.Authentication
{
    public class AuthInstance
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("session_token")]
        public string SessionToken { get; set; }

        [JsonProperty("access_token_expires_at")]
        public string AccessTokenExpiresAt { get; set; }
    }
}