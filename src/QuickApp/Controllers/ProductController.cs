using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using DAL;
using AutoMapper;
using QuickApp.ViewModels;
using DAL.Models;
using CommonData.RabbitQueue;
using CommonData.Models;
using StackExchange.Redis;

namespace QuickApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        readonly ILogger _logger;
        private readonly IBus _busControl;
        private readonly IDatabase _rediscache;

        public ProductController(IUnitOfWork unitOfWork, ILogger<CustomerController> logger, IBus busControl, IDatabase rediscache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _busControl = busControl;
            _rediscache = rediscache;
        }

        [HttpGet("getallproducts")]
        [ProducesResponseType(200, Type = typeof(ProductViewModel))]
        public IActionResult Get()
        {
            var allProducts = _unitOfWork.Products.GetAll();
            //put to cache

            if(!_rediscache.KeyExists(allProducts.FirstOrDefault().Id.ToString()))
            {
                foreach(Product p in allProducts)
                {
                    _rediscache.SetAdd(p.Id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(p));
                }
            }

            return Ok(Mapper.Map<IEnumerable<ProductViewModel>>(allProducts));
        }

        [HttpPost("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public IActionResult AddProduct([FromBody] Product product)
        {
            ProductRequest request = new ProductRequest();
            product.CreatedDate = product.DateCreated;
            product.UpdatedDate = product.DateModified;
            _unitOfWork.Products.Add(product);
            request.Message = "New Product - " + product.Name;
            _busControl.SendAsync(Queue.Processing, request);
            return Ok();
        }

        [HttpGet("getproduct/{id}")]
        [ProducesResponseType(200, Type = typeof(ProductViewModel))]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult ProductById(int id)
        {
            if (!_rediscache.KeyExists(id.ToString()))
            {
                var product = _unitOfWork.Products.Find(e => e.Id == id);
                string serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(product.FirstOrDefault());
                _rediscache.SetAdd(id.ToString(), serializedData);
            }
            var prd_ser = _rediscache.SetMembers(id.ToString()).FirstOrDefault().ToString();
            if (string.IsNullOrEmpty(prd_ser))
            {
                var prdct = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(prd_ser);
                return Ok("No Such Product Exists");
            }
            else
            {
                var prdct = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(prd_ser);
                return Ok(Mapper.Map<ProductViewModel>(prdct));
            }
            
        }
    }
}
