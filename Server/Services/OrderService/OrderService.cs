
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

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));

            response.Data = orderResponse;

            return response;
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
                UserId = _authService.GetUserId()
            };
            _context.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()));
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Success = true };
        }
        
    }
}
