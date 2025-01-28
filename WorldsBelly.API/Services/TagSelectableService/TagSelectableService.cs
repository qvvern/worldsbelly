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
    public class TagSelectableService : ITagSelectableService
    {
        private ITagSelectableRepository _tagSelectableRepository;

        public TagSelectableService(ITagSelectableRepository tagSelectableRepository)
        {
            _tagSelectableRepository = tagSelectableRepository;
        }

        public async Task<ActionResult<ICollection<TagSelectableChoiceOrderView>>> GetTagSelectableChoiceOrdersAsync()
        {
            var response = _tagSelectableRepository.GetTagSelectableChoiceOrdersAsync().ToList();
            return response.Select(ResponseMapper.Map).ToList();
        }

    }
}
