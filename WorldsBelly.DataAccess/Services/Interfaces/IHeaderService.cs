using System;

namespace WorldsBelly.DataAccess.Services.Interfaces
{
    public interface IHeaderService
    {
        Guid GetUserId();
        int? GetLanguageId();
        string GetToken();
        string GetDisplayUrl();
        string GetCookie(string key);
        void SetCookie(string key, string value, DateTime? expire);
    }
}
