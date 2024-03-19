namespace Ecommerce.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
        Task<ServiceResponse<Category>> GetCategory(int categoryId);
        Task<ServiceResponse<List<Category>>> GetAdminCategories();
        Task<ServiceResponse<List<Category>>> AddCategory(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategory(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategory(int id);
    }
}
