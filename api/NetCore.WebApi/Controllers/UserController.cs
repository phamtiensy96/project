using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.System;
using NetCore.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace NetCore.WebApi.Controllers
{
    
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
         
            var data = await _userService.GetAllAsync();
            return new OkObjectResult(data);
        }

        [HttpGet("UpdateStatus/{id}")]
        public async Task<ActionResult> UpdateStatusAsync(string id)
        {
            await _userService.UpdateStatusAsync(id);
            return new OkObjectResult(true);
        }


        [HttpPost]
        public async Task<ActionResult> SaveEntity([FromBody]AppUserViewModel appUserViewModel)
        {
            IdentityResult result = null;

            if (appUserViewModel.Id == null)
            {
                result = await _userService.AddAsync(appUserViewModel);
            }
            else
            {
                result = await _userService.UpdateAsync(appUserViewModel);
            }

            if (result.Succeeded)
            {
                return new OkObjectResult(true);
            }
            else
            {
                var errorList = string.Join(" || ", result.Errors.Select(x => x.Description).ToList());
                return new BadRequestObjectResult(errorList);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return new OkObjectResult(true);
        }

    }
}