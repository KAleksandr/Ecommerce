using Blazored.LocalStorage;
using Ecommerce.Shared.DTO;
using Ecommerce.Shared.Model;

namespace Ecommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider)
        {

            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsAuthenticated())
            {
                Console.WriteLine("User is authenticated");
            }
            else
            {
                Console.WriteLine("User is not authenticated");
            }
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            var sameItem = cart.Find(x => x.ProductTypeId == cartItem.ProductTypeId && x.ProductId == cartItem.ProductId);

            if (sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _localStorage.SetItemAsync("cart", cart);
            await GetCartItemsCount();
        }



        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemsCount();
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
        }

        public async Task GetCartItemsCount()
        {
            if (await IsAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;
                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0 );
            }
            OnChange.Invoke();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {

            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null)
            {
                return new List<CartProductResponse>();
            }
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);

            var cartProducts =
               await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;


        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
            var carItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
            if (carItem != null)
            {
                cart.Remove(carItem);
                await _localStorage.SetItemAsync("cart", cart);
                await GetCartItemsCount();
            }

        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }
            await _http.PostAsJsonAsync("api/cart", localCart);
            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
            var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }

        }
        private async Task<bool> IsAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
