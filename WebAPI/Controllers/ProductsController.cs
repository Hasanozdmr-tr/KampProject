using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]   // Erişilecek path.
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Dependency Chain.
            Thread.Sleep(1000);
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }

                return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
           

            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("getallbycategory")]
        public IActionResult GetAllByCategory(int categoryId)
        {


            var result = _productService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }


    }
}
