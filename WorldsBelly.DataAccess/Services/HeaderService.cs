using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using RestSharp;
using System;
using System.Net.Http;
using System.Security.Policy;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensitions;

namespace WorldsBelly.DataAccess.Services
{
    public class HeaderService : IHeaderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public Guid GetUserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.User.GetLoggedInUserId());
        }
        public int? GetLanguageId()
        {
            _httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue("Language-Id", out var headerLanguageId);
            if (String.IsNullOrEmpty(headerLanguageId)) return null; // throw new Exception("Cannot find language from header");
            return Int32.Parse(headerLanguageId);
        }
        public string GetToken()
        {
            _httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue("Authorization", out var authorization);
            return authorization;
        }
        public string GetDisplayUrl()
        {
            return _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();
        }
        public string GetCookie(string key)
        {
            var test = _httpContextAccessor.HttpContext.Request.Cookies;
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies[key];

            return cookieValueFromContext;
        }
        public void SetCookie(string key, string value, DateTime? expire)
        {
            if(expire != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = expire,
                    SameSite = SameSiteMode.None,
                    HttpOnly = true
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
            }
            else
            {
                _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value);
            }
        }
    }
}
