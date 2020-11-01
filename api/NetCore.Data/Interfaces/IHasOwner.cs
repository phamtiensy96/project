using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { set; get; }


    }
}
