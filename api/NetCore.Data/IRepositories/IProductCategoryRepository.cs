using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Entities;
using NetCore.Infrastructure.Interfaces;

namespace NetCore.Data.IRepositories
{
   public interface IProductCategoryRepository:IRepository<ProductCategory,int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
