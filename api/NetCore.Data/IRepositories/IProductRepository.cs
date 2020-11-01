using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Entities;
using NetCore.Infrastructure.Interfaces;

namespace NetCore.Data.IRepositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
