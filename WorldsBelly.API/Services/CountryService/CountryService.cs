using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Services
{
    public class CountryService : ICountryService
    {
        private ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<ActionResult<ICollection<CountryView>>> GetCountriesAsync(int? languageId)
        {
            var response = await _countryRepository.GetCountriesAsync(languageId);
            return response.Select(ResponseMapper.Map).ToList();
        }

    }
}
