using NetCore.Data.Entities;
using NetCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.IRepositories
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
    }
}
