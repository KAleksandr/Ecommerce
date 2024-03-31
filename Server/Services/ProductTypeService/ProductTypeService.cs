
using Ecommerce.Client.Pages.Admin;
using Ecommerce.Shared.Model;

namespace Ecommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> DeleteProductType(int id)
        {
            var dbProductType = await _context.ProductTypes.FirstOrDefaultAsync(pt => pt.Id == id);
            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>> { Success = false, Message = "Sorry, but this productType does not exist" };
            }
           // dbProductType.Delited = true;            
            await _context.SaveChangesAsync();
            return await GetProductTypes();

            
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>>{ Data = productTypes };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _context.ProductTypes.FirstOrDefaultAsync(pt=> pt.Id == productType.Id);
            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found"
            };

        }
            
        dbProductType.Name = productType.Name;              
            
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }
    }
}
