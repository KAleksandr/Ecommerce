using Ecommerce.Shared.Model;

namespace Ecommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
           _http = http;
        }
        public List<Category> Categories { get; set; } = new List<Category>();
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
    }
}
