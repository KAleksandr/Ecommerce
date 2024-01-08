using Ecommerce.Shared.Model;

namespace Ecommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var response = new ServiceResponse<List<Category>>()
            {
                Data = await _context.Categories.ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<Category>> GetCategoryAsync(int categoryId)
        {
            var response = new ServiceResponse<Category>();
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this category does not exist";
            }
            else { response.Data = category; }

            return response;
        }
    }
}
