﻿
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

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrdersDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id ==  orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();
            if(order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
                return response;
            }
            var orderDetailsResponse = new OrderDetailsResponse
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };
            order.OrderItems.ForEach(item =>
            {
                orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
                {
                    ProductId = item.ProductId,
                    ImageUrl = item.Product.ImageUrl,
                    ProductType = item.ProductType.Name,
                    Title = item.Product.Title,
                    TotalPrice = item.TotalPrice,
                    Quantity = item.Quantity
                });
            });
            response.Data = orderDetailsResponse;
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
