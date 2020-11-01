using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.Interfaces
{
    public interface IDateTracking
    {
        DateTime DateCreated { set; get; }

        DateTime DateModified { set; get; }
    }
}
