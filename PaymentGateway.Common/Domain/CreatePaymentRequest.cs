namespace PaymentGateway.Common.Domain
{
    public class CreatePaymentRequest
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Cvv { get; set; }
        public bool? DebugExpectSuccess { get; set; }
    }
}
