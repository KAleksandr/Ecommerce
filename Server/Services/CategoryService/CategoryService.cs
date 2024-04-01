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

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
          Category category =  await GetCategoryById(id);
            if(category == null)
            {
                return new ServiceResponse<List<Category>> { Success = false, Message = "Sorry, but this category does not exist" };
            }
            category.Deleted = true;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        private async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var categories = await _context.Categories.Where(c => !c.Deleted).ToListAsync();

            return new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.Where(c => !c.Deleted && c.Visible).ToListAsync();
            
            return new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<Category>> GetCategory(int categoryId)
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

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            Category dbCategory = await GetCategoryById(category.Id);
            if(dbCategory == null)
            {
                return new ServiceResponse<List<Category>> { Success = false, Message = "Sorry, but this category does not exist" };
            }
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }
    }
}
