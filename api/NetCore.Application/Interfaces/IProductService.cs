using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetCore.Application.ViewModels.Product;
using NetCore.Utilities.Dtos;

namespace NetCore.Application.Interfaces
{
    public interface IProductService: IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
    }
}
