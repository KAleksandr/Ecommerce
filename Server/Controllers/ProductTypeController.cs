﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
        {
          var productTypes =  await _productTypeService.GetProductTypes();
            return Ok(productTypes);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
        {
            var productTypes = await _productTypeService.AddProductType(productType);
            return Ok(productTypes);
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType)
        {
            var productTypes = await _productTypeService.UpdateProductType(productType);
            return Ok(productTypes);
        }
        [HttpDelete("/{id}")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> DeleteProductType(int id)
        {
            var productTypes = await _productTypeService.DeleteProductType(id);
            return Ok(productTypes);
        }
    }
}
