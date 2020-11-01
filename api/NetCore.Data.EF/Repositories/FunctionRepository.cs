using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Entities;
using NetCore.Data.IRepositories;

namespace NetCore.Data.EF.Repositories
{
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
