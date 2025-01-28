using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface ICountryService
    {
        Task<ActionResult<ICollection<CountryView>>> GetCountriesAsync(int? languageId);
    }
}
