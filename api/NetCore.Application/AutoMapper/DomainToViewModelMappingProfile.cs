using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Application.ViewModels.Product;
using NetCore.Application.ViewModels.System;
using NetCore.Data.Entities;

namespace NetCore.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile:Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Permission, PermissionViewModel>();

            CreateMap<Product, ProductViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
        }
    }
}
