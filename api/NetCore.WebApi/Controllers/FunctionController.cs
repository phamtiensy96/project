﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCore.WebApi.Controllers
{
    public class FunctionController : ApiController
    {
        private IFunctionService _functionService;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;


        public FunctionController(IFunctionService functionService, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _functionService = functionService;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet("filter/{filter}")]
        public IActionResult GetAllFillter(string filter)
        {
            var model = _functionService.GetAll(filter);
            return new ObjectResult(model);
        }

        [HttpGet("GetAllFunction/{roleId}")]
        public async Task<IActionResult> GetAllFunctionByRoleId(Guid roleId)
        {
            //var model = await _functionService.GetAll(string.Empty);
            //var rootFunctions = model.Where(c => c.ParentId == null);
            //var items = new List<FunctionViewModel>();
            //foreach (var function in rootFunctions)
            //{
            //    //add the parent category to the item list
            //    items.Add(function);
            //    //now get all its children (separate Category in case you need recursion)
            //    GetByParentId(model.ToList(), function, items);
            //}

            var model =await _functionService.GetAllFunctionRole(roleId);
            var rootFunctions = model.Where(c => c.ParentId == null);
            var items = new List<PermissionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var model = _functionService.GetAll(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel functionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(functionVm.Id))
                {
                    _functionService.Add(functionVm);
                }
                else
                {
                    _functionService.Update(functionVm);
                }
                _functionService.Save();
                return new OkObjectResult(functionVm);
            }
        }

        [HttpPost]
        public IActionResult UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _functionService.UpdateParentId(sourceId, targetId, items);
                    _functionService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _functionService.ReOrder(sourceId, targetId);
                    _functionService.Save();
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                _functionService.Delete(id);
                _functionService.Save();
                return new OkObjectResult(id);
            }
        }

        #region Private Functions
        private void GetByParentId(IEnumerable<PermissionViewModel> allFunctions,
            PermissionViewModel parent, IList<PermissionViewModel> items)
        {
            var functionsEntities = allFunctions as PermissionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.FunctionId);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
        #endregion
    }
}