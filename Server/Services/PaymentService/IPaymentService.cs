using Stripe.Checkout;

namespace Ecommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheackoutSessionOut();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}
