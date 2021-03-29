using Business.Abstract;
using Business.CCS;
using Business.Constant;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcern.Validation;
using Core.Utilities.Results;
using Core.Utilities.Business;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Performance;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            //dependency injection
            _productDal = productDal;
            _categoryService = categoryService;


        }
        [SecuredOperation("admin,product.add")] //bu bir aspect tir.
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")] //sadece IProductService teki getleri siler.
        public IResult Add(Product product)
        {
            //ValidationTool.Validate(new ProductValidator(), product);
            //Bir kategoride en fazla 10 ürün olabilir iş kuralı
            // Eğer mevcut kategori sayısı 15 i geçtiyse sisteme yeni ürün eklenemez.

           IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfCategoryLimitExceded());

            if(result!=null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
       
        [CacheAspect] //key (cache verdiğimiz isim), value
        public IDataResult<List<Product>> GetAll()
        {
            if(DateTime.Now.Hour==20)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenenceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=> p.CategoryId==categoryId), Messages.ProductsListed);
        }

        [CacheAspect]
        [PerformanceAspect(5)] // 5 saniye yı geçerse beni uyar demek bu.
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=>p.ProductId==productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice>= min && p.UnitPrice<=max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }


        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")] //sadece IProductService teki getleri siler.
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {// Bu iş kuralı parçacığı oyüzden sadece bu classta kullanılacak. Dolayısıyla private yapılır.
            //Eğer farklı classlarda da kullanılacaksa bu bir servis olmalıdır. IProductService e taşımalısın.
            // burası db de Select count(*) from product where CategoryId gibi sorgu yapar.
            var _categorySum = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (_categorySum >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any(); //any var mı demek
            if(result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if(result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();

        }
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
                if(product.UnitPrice<10)
            {
                throw new Exception(" ");
            }
            Add(product);
                return null;


        }

        public IDataResult<List<Product>> GetAllByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
