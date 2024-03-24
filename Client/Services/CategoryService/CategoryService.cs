
using Ecommerce.Shared;
using Ecommerce.Shared.Model;

namespace Ecommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public event Action OnChange;

        public CategoryService(HttpClient http)
        {
           _http = http;
        }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Category> AdminCategories { get; set; } = new List<Category>();

        public async Task GetCatigories()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (result != null && result.Data != null)
            {
                Categories = result.Data;
            }

        }
        public async Task<ServiceResponse<Category>> GetCategory(int categoryId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Category>>($"api/category/{categoryId}");

            return result;
        }

        public async Task GetAdminCategories()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
            if (result != null && result.Data != null)
            {
                AdminCategories = result.Data;
            }
        }

        public async Task AddCategory(Category category)
        {
            var response = await _http.PostAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCatigories();
            OnChange.Invoke();
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await _http.PutAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCatigories();
            OnChange.Invoke();
        }

        public async Task DeleteCategory(int id)
        {
            var response = await _http.DeleteAsync($"api/Category/admin/{id}");
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCatigories();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
           var category = new Category {IsNew = true, Editing = true };
            AdminCategories.Add(category);
            OnChange.Invoke();
            return category;
        }
    }
}
