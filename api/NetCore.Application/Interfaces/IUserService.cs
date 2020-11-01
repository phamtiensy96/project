using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetCore.Application.ViewModels.System;
using NetCore.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;

namespace NetCore.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task UpdateStatusAsync(string id);

         Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);


        Task<IdentityResult> UpdateAsync(AppUserViewModel userVm);

    }
}
