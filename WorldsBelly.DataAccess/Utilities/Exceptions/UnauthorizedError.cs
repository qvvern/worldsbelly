using System.Net;

namespace WorldsBelly.DataAccess.Utilities.Exceptions
{
    public class UnauthorizedError : ApiError
    {
        public UnauthorizedError() : base(401, HttpStatusCode.Unauthorized.ToString())
        {
        }

        public UnauthorizedError(string message) : base(401, HttpStatusCode.Unauthorized.ToString(), message)
        {
        }
    }
}