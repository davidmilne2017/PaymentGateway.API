using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class MockedBankHttpRepository : IBankHttpRepository
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var expectedResult = createPaymentRequest.DebugExpectSuccess ?? true;
            var createPaymentResponse = new CreatePaymentResponse()
            {
                Success = expectedResult,
                TransactionId = Guid.NewGuid().ToString(),
                Errors = expectedResult ? Array.Empty<string>() : new string[] { "There has been an error processing your payment"}
            };
            return createPaymentResponse;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<PaymentDetailsResponse> RetrievePaymentDetailsAsync(PaymentDetailsRequest paymentDetailsRequest)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var expectedStatusCode = paymentDetailsRequest.DebugExpectedStatusCode == PaymentStatusCode.NotSet ? PaymentStatusCode.Success 
                : paymentDetailsRequest.DebugExpectedStatusCode;
            var paymentDetailsResponse = new PaymentDetailsResponse();
            if (expectedStatusCode == PaymentStatusCode.Success || expectedStatusCode == PaymentStatusCode.Failure)
            {
                paymentDetailsResponse = CreatePayment(paymentDetailsResponse, expectedStatusCode);
            } else
            {
                paymentDetailsResponse.StatusCode = PaymentStatusCode.TransactionIdNotRecognised;
            }
            
            return paymentDetailsResponse;
        }

        private PaymentDetailsResponse CreatePayment(PaymentDetailsResponse paymentDetailsResponse, PaymentStatusCode expectedStatusCode)
        {

            Random RNG = new Random();
            var builder = new StringBuilder();
            builder.Append("XXXXXXXXXXXX");
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }

            paymentDetailsResponse.CardHolderName = "Mr Card Holder";
            paymentDetailsResponse.CardNumber = builder.ToString();
            paymentDetailsResponse.ExpiryMonth = "12";
            paymentDetailsResponse.ExpiryYear = "2025";
            paymentDetailsResponse.Amount = 100.00M;
            paymentDetailsResponse.CurrencyCode = "GBP";
            paymentDetailsResponse.StatusCode = expectedStatusCode;
            return paymentDetailsResponse;
        }
    }
}
