using Newtonsoft.Json;
using System.Net;

namespace WorldsBelly.DataAccess.Utilities.Exceptions
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string Message { get; set; } = "An error has occurred on the server side";


        public string ToJsonString()
            => JsonConvert.SerializeObject(this);
    }
}
