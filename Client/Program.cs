global using Ecommerce.Shared;
global using Ecommerce.Shared.Model;
global using System.Net.Http.Json;
global using Ecommerce.Client.Services.ProductService;
global using Ecommerce.Client.Services.CategoryService;
global using Ecommerce.Client.Services.CartService;
global using Ecommerce.Client.Services.AuthService;
global using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;


namespace Ecommerce.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddBlazoredLocalStorage();
           
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IProductService, ProductService>();  
            builder.Services.AddScoped<ICategoryService, CategoryService>();  
            builder.Services.AddScoped<ICartService, CartService>();  
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            await builder.Build().RunAsync();
        }
    }
}