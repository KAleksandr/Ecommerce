using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Get list products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProduct()
        {   
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }
    }
}
