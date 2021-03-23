using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Dtos;

namespace PaymentGateway.Common.Mappers
{
    public static class CreatePaymentMappers
    {
        public static CreatePaymentRequest MapPaymentRequestDtoToPaymentRequest(this CreatePaymentRequestDto createPaymentRequestDto)
        {
            return new CreatePaymentRequest()
            {
                CardHolderName = createPaymentRequestDto.CardHolderName,
                CardNumber = createPaymentRequestDto.CardNumber,
                ExpiryMonth = createPaymentRequestDto.ExpiryMonth,
                ExpiryYear = createPaymentRequestDto.ExpiryYear,
                Amount = createPaymentRequestDto.Amount,
                CurrencyCode = createPaymentRequestDto.CurrencyCode,
                Cvv = createPaymentRequestDto.CVV
            };
        }        

        public static CreatePaymentResponseDto MapPaymentResponseToPaymentResponseDto(this CreatePaymentResponse createPaymentResponse)
        {
            return new CreatePaymentResponseDto()
            {
                Success = createPaymentResponse.Success,
                TransactionId = createPaymentResponse.TransactionId,
                Errors = createPaymentResponse.Errors
            };
        }

    }
}
