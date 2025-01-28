using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Services
{
    public class LanguageService : ILanguageService
    {
        private ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<ActionResult<ICollection<LanguageView>>> GetLanguagesAsync()
        {
            var response = await _languageRepository.GetLanguagesAsync();
            response = response.Where(l => !l.IsHidden);
            return response.Select(ResponseMapper.Map).ToList();
        }
    }
}
