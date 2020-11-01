using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Entities;
using NetCore.Data.IRepositories;

namespace NetCore.Data.EF.Repositories
{
    public class ProductRepository : EFRepository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
