using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCore.Data.Entities;
using NetCore.Data.IRepositories;

namespace NetCore.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>,IProductCategoryRepository
    {
        AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}
