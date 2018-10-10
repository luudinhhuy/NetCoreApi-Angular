﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Mobile.Common;
using API.Mobile.Infrastructure.Middlewares;
using API.Mobile.Models;
using API.Mobile.Repository;
using API.Mobile.Resources;
using API.Mobile.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace API.Mobile.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [MiddlewareFilter(typeof(LocalizationMiddleware))]
    [Route("api/v{version:apiVersion}/{lang}/[controller]")]
    public class UserController : ControllerBase
    {
        private User user = FakeData.user;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IStringLocalizer stringLocalizer;
        private IMemoryCache _cache;
        public UserController(IConfiguration config, IUserRepository user, IStringLocalizer<LanguageSub> stringLocal, IMemoryCache memoryCache)
        {
            configuration = config;
            userRepository = user;
            stringLocalizer = stringLocal;
            _cache = memoryCache;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                var caheUser = _cache.Set("users", FakeData.users);
                var users = (List<User>)_cache.Get("users");
                var user = users.FirstOrDefault(x => x.StaffId == model.StaffId);
                if (user == null)
                {
                    return Ok(stringLocalizer[LanguageSub.MSG_USER_NOT_FOUND]);
                }
                if (model.StaffId == user.StaffId && model.Password == user.Password)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.StaffId),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserId", user.UserId),
                        new Claim("Role", user.Role)
                    };
                    var token = new JwtSecurityToken
                    (
                        issuer: configuration["Issuer"],
                        audience: configuration["Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(2),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"])),
                                SecurityAlgorithms.HmacSha256)
                    );
                    var u = new { user.StaffId, user.Role, user.UserId, user.Email };
                    return Ok(new { u, token });
                }
                else
                {
                    return Ok(stringLocalizer[LanguageSub.MSG_DATA_NOT_FOUND]);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(stringLocalizer[Ex.Message]);
                throw;
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("Profile")]
        public User Profile(string userId)
        {
            var result = userRepository.Get(userId);
            result.Password = null;
            return result;
        }

        [HttpPost]
        //[Authorize]
        [Route("ChangePassword")]
        public LocalizedString ChangePassword(ChangePasswordModel model)
        {
            if(model.NewPassword.Length < 6)
            {
                return stringLocalizer[LanguageSub.NEW_PASSWORD_MUST_LEAST_LENGTH];
            }
            if (model.NewPassword == model.OldPassword)
            {
                return stringLocalizer[LanguageSub.PASSWORD_DUPLICATE];
            }
            var u = userRepository.Get(model.UserId);
            if (u != null)
            {
                if (u.Password == model.OldPassword)
                {
                    FakeData.users.FirstOrDefault(x => x.UserId == model.UserId).Password = model.NewPassword;
                    return stringLocalizer[LanguageSub.CHANGE_PASSWORD_SUCCESS];
                }
                else
                {
                    return stringLocalizer[LanguageSub.OLD_PASSWORD_NOT_CORRECT];
                }
            }
            else
            {
                return stringLocalizer[LanguageSub.MSG_DATA_NOT_FOUND];
            }
        }
    }
}