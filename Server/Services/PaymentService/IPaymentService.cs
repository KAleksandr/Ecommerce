using Stripe.Checkout;

namespace Ecommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheackoutSessionOut();
    }
}
