

using Ecommerce.Shared.Model;

namespace Ecommerce.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var result = await _http.PostAsJsonAsync("api/producttype", productType);
            ProductTypes = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
            
        }

        public ProductType CreateNewProductType()
        {
           ProductType productType = new ProductType { IsNew = true, Editing = true};
            ProductTypes.Add(productType);
            OnChange.Invoke();
            return productType;
        }

        public async Task DeleteProductType(int id)
        {
            var result = await _http.DeleteAsync($"api/producttype/{id}");
            ProductTypes = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
           
        }

        public async Task GetProductTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
            ProductTypes = result.Data;

        }

        public async Task UpdateProductType(ProductType productType)
        {
            var result = await _http.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
            
        }
    }
}
