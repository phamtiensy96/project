using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.System;
using NetCore.Data.IRepositories;
using NetCore.Application.AutoMapper;
using NetCore.Data.Entities;
using AutoMapper;
using NetCore.Data.Enums;
using NetCore.Infrastructure.Interfaces;

namespace NetCore.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        IFunctionRepository _functionRepository;
        private IPermissionRepository _permissionRepository;
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork, IMapper mapper, MapperConfiguration mapperConfig, IPermissionRepository permissionRepository)
        {
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
            _unitOfWork = unitOfWork;
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            _functionRepository.Add(function);
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindSingle(x => x.Id == id);
            return _mapper.Map<Function, FunctionViewModel>(function);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));
            return query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>(_mapperConfig).ToListAsync();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _functionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>(_mapperConfig);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {

            var functionDb = _functionRepository.FindById(functionVm.Id);
            var function = _mapper.Map<Function>(functionVm);
        }

        public void ReOrder(string sourceId, string targetId)
        {

            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);

        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public Task<List<PermissionViewModel>> GetAllFunctionRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll(x=>x.RoleId == roleId);

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        select new PermissionViewModel()
                        {
                            FunctionId = f.Id,
                            FunctionName = f.Name,
                            ParentId = f.ParentId,
                            CanCreate = p != null ? p.CanCreate : false,
                            CanDelete = p != null ? p.CanDelete : false,
                            CanRead = p != null ? p.CanRead : false,
                            CanUpdate = p != null ? p.CanUpdate : false
                        };
            return   query.ToListAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
