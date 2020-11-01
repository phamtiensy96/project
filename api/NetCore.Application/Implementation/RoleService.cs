using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetCore.Application.AutoMapper;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.System;
using NetCore.Data.Entities;
using NetCore.Data.IRepositories;
using NetCore.Infrastructure.Interfaces;
using NetCore.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManager;
        private UserManager<AppUser> _userManager;
        private IFunctionRepository _functionRepository;
        private IPermissionRepository _permissionRepository;
        private IMapper _mapper;
        private MapperConfiguration _configMapper;
        private IUnitOfWork _unitOfWork;
        public RoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork,
         IFunctionRepository functionRepository, IPermissionRepository permissionRepository ,IMapper mapper, MapperConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _mapper =mapper;
            _configMapper = config;
        }
        public async Task<bool> AddAsync(AppRoleViewModel roleVm)
        {
            var role = new AppRole()
            {
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>(_configMapper).ToListAsync();
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<AppRoleViewModel>(_configMapper).ToList();
            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return _mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null ? p.CanCreate : false,
                            CanDelete = p != null ? p.CanDelete : false,
                            CanRead = p != null ? p.CanRead : false,
                            CanUpdate = p != null ? p.CanUpdate : false
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = _mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
            var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.RemoveMultiple(oldPermission);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id);
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }

        public async Task<List<string>> GetRolesById(Guid? id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        //public async Task RemoveRolesFromUser(string userId, string[] roles)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var roleIds = _roleManager.Roles.Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
        //    List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();
        //    foreach (var roleId in roleIds)
        //    {
        //        userRoles.Add(new IdentityUserRole<Guid> { RoleId = roleId, UserId = user.Id });
        //    }
        //    _context.UserRoles.RemoveRange(userRoles);
        //    await _context.SaveChangesAsync();
        //}

    }
}
