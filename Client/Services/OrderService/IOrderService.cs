using Ecommerce.Shared.DTO;

namespace Ecommerce.Client.Services.OrderService
{
    public interface IOrderService
    {        
        Task<string> PlaceOrder();
        Task<List<OrderOverviewResponse>> GetOrders();
        Task<OrderDetailsResponse> GetOrdersDetails(int orderId);
    }
}
