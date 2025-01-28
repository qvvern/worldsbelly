
namespace WorldsBelly.Domain.Models.Authentication
{
    public class OktaAuthResponse
    {
        public string ExpiresAt { get; set; }
        public string Status { get; set; }
        public string sessionToken { get; set; }
 
    }
}
