using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCore.Application.Interfaces;
using NetCore.Application.ViewModels.Product;
using NetCore.Data.Entities;
using NetCore.Data.Enums;
using NetCore.Data.IRepositories;
using NetCore.Infrastructure.Interfaces;
using NetCore.Application.AutoMapper;

namespace NetCore.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository,IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>(AutoMapperConfig.RegisterMappings()).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>(AutoMapperConfig.RegisterMappings()).ToList();
            }
            else
            {
                return _productCategoryRepository.FindAll()
                      .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>(AutoMapperConfig.RegisterMappings()).ToList();
            }
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x=>x.Status==Status.Active)
                      .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>(AutoMapperConfig.RegisterMappings()).ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return _mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Products)
                 .OrderBy(x => x.HomeOrder).Take(top).ProjectTo<ProductCategoryViewModel>(AutoMapperConfig.RegisterMappings());
            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products= _productCategoryRepository
                //    .FindAll(x=>x.HomeFlag==true&&x.Cate)
            }

            return categories;
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}
