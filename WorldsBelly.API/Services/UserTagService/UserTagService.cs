//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WorldsBelly.API.Models;
//using WorldsBelly.API.Services.Interfaces;
//using WorldsBelly.API.Utilities.Mappers;
//using WorldsBelly.DataAccess.Entities;
//using WorldsBelly.DataAccess.Repository.Interfaces;
//using WorldsBelly.DataAccess.Utilities.Extensions;

//namespace WorldsBelly.API.Services
//{
//    public class UserTagService : IUserTagService
//    {
//        private IUserTagRepository _userTagRepository;

//        public UserTagService(IUserTagRepository userTagRepository)
//        {
//            _userTagRepository = userTagRepository;
//        }

//        public async Task<ActionResult<ICollection<UserTagView>>> GetUserTagsAsync(int? startAt, int? amount, string search)
//        {
//            var response = _userTagRepository.GetUserTagsAsync(startAt, amount, search);
//            return response.Select(ResponseMapper.Map).ToList();
//        }

//        //public async Task<ActionResult<ICollection<UserTagView>>> CreateTagsAsync(ICollection<UserTagView> tags)
//        //{
//        //    ICollection<UserTag> usertags = tags.Select(RequestMapper.Map).ToList();
//        //    var response = await _userTagRepository.GetOrCreateUserTagsAsync(usertags);
//        //    return response.Select(ResponseMapper.Map).ToList();
//        //}

//    }
//}
