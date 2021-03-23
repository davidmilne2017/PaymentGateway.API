using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Dtos;
using System;

namespace PaymentGateway.Common.Mappers
{
    public static class PaymentDetailsMapper
    {
        public static PaymentDetailsRequest MapPaymentDetailsRequestDtoToPaymentDetailsRequest(this PaymentDetailsRequestDto paymentDetailsRequestDto)
        {
            return new PaymentDetailsRequest()
            {
                TransactionId = paymentDetailsRequestDto.TransactionId
            };
        }

        public static PaymentDetailsResponseDto MapPaymentDetailsResponseToPaymentDetailsResponseDto(this PaymentDetailsResponse paymentDetailsResponse)
        {
            return new PaymentDetailsResponseDto()
            {
                CardHolderName = paymentDetailsResponse.CardHolderName,
                CardNumber = paymentDetailsResponse.CardNumber,
                ExpiryMonth = paymentDetailsResponse.ExpiryMonth,
                ExpiryYear = paymentDetailsResponse.ExpiryYear,
                Amount = paymentDetailsResponse.Amount,
                CurrencyCode = paymentDetailsResponse.CurrencyCode,
                StatusCode = paymentDetailsResponse.StatusCode.ToString()
            };
        }
    }
}
