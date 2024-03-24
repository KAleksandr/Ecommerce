namespace Ecommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action OnChange;
        public List<Category> Categories { get; set; }
        public List<Category> AdminCategories { get; set; }
        Task GetCatigories();
        Task<ServiceResponse<Category>> GetCategory(int categoryId);
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
        Category CreateNewCategory();
    }
}
