using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { set; get; }
    }
}
