using NetCore.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.WebApi.Models
{
    public class PermissionAddViewModel
    {
        public List<PermissionViewModel> listPermmission  { get; set; }
        public Guid roleId { get; set; }
    }
}
