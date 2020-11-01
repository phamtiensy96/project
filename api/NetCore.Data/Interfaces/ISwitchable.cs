using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Enums;

namespace NetCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
