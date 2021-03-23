using PaymentGateway.Common.Domain;

namespace PaymentGateway.Common.Dtos
{
    public class PaymentDetailsResponseDto
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string StatusCode { get; set; }
    }
}
