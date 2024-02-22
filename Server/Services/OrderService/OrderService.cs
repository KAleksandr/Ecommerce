
using System.Security.Claims;

namespace Ecommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
       

        public OrderService(DataContext context, ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            
        }
        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();
            products.ForEach(p =>
            {
                totalPrice += p.Price * p.Quantity;
                orderItems.Add(new OrderItem
                {
                    ProductId = p.ProductId,
                    ProductTypeId = p.ProductTypeId,
                    TotalPrice = p.Price * p.Quantity,
                    Quantity = p.Quantity,
                });
            });
            var order = new Order
            {
                TotalPrice = totalPrice,
                OrderItems = orderItems,
                UserId = GetUserId()
            };
            _context.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == GetUserId()));
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Success = true };
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
