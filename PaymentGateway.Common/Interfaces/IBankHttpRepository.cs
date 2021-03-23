using PaymentGateway.Common.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Common.Interfaces
{
    public interface IBankHttpRepository
    {
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest);

        Task<PaymentDetailsResponse> RetrievePaymentDetailsAsync(PaymentDetailsRequest paymentDetailsRequest);
    }
}
