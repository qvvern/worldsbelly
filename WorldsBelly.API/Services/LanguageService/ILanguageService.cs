using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<ActionResult<ICollection<LanguageView>>> GetLanguagesAsync();
    }
}
