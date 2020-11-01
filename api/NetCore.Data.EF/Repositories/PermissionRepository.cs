using NetCore.Data.Entities;
using NetCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.EF.Repositories
{
    public class PermissionRepository : EFRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
