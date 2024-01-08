namespace Ecommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        public List<Category> Categories { get; set; }
        Task GetCatigories();
        Task<ServiceResponse<Category>> GetCategory(int categoryId);
    }
}
