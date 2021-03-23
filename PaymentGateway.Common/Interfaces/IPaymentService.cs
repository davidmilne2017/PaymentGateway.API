using PaymentGateway.Common.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Common.Interfaces
{
    public interface IPaymentService
    {
        string[] ValidateCreatePaymentRequest(CreatePaymentRequest createPaymentRequest);
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest);
        Task<PaymentDetailsResponse> RetrievePaymentDetailsAsync(PaymentDetailsRequest paymentDetailsRequest);
    }
}
