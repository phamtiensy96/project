using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.Product;
using NetCore.Application.ViewModels.System;
using NetCore.Data.Entities;
using NetCore.Data.IRepositories;
using NetCore.Utilities.Dtos;
using NetCore.Application.AutoMapper;
using NetCore.Data.Enums;

namespace NetCore.Application.Implementation
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;
        private IRoleService _roleService;
        private IMapper _mapper;
        private MapperConfiguration _configMapper;
        public UserService(UserManager<AppUser> userManager, IRoleService roleService, IMapper mapper, MapperConfiguration config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configMapper = config;
            _roleService = roleService;
        }

        public async Task<IdentityResult> AddAsync(AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                Status = userVm.Status,
                Address = userVm.Address
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded)
            {
                if (userVm.Roles!=null && userVm.Roles.Count > 0)
                {
                    var appUser = await _userManager.FindByNameAsync(user.UserName);
                    if (appUser != null)
                        await _userManager.AddToRolesAsync(appUser, userVm.Roles);
                }
            }
            return result;

        }

        public async Task<IdentityResult> UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());

            if (result.Succeeded)
            {

                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                var resultRole = await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                //Update user detail

                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Avatar = userVm.Avatar;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                user.Address = userVm.Address;
                if (!string.IsNullOrEmpty(userVm.Password))
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userVm.Password);
                }
                await _userManager.UpdateAsync(user);

            }

            return result;

        }


        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task UpdateStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Status= (user.Status==Status.Active) ? Status.InActive : Status.Active;
            await _userManager.UpdateAsync(user);
        }


        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            var users = await _userManager.Users.ProjectTo<AppUserViewModel>(_configMapper).ToListAsync();
            var getUserRole = users
                .Select(async ev =>
                {
                    ev.Roles = await _roleService.GetRolesById(ev.Id);
                    return ev;
                })
                .Select(x =>
                {
                    return x.Result;
                }).ToList();

            return getUserRole;
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated

            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }


    }
}
