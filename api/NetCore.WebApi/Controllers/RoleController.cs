using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.System;
using NetCore.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCore.WebApi.Controllers
{
    [Authorize]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public RoleController(IRoleService roleService, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _roleService = roleService;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();

            return new OkObjectResult(model);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet("{keyword}/{page}/{pageSize}")]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (roleVm.Id != null)
            {
                await _roleService.AddAsync(roleVm);
            }
            else
            {
                await _roleService.UpdateAsync(roleVm);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }


        [HttpGet("FunctionRole/{roleId}")]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost("SavePermission")]
        public IActionResult SavePermission([FromBody]PermissionAddViewModel model)
        {
            _roleService.SavePermission(model.listPermmission, model.roleId);
            return new OkResult();
        }
    }

}
