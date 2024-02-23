
using System.Security.Claims;

namespace Ecommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
       
        private readonly IAuthService _authService;

        public OrderService(DataContext context, ICartService cartService,  IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
           
            _authService = authService;
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
                UserId = _authService.UserId()
            };
            _context.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == _authService.UserId()));
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Success = true };
        }
        
    }
}
