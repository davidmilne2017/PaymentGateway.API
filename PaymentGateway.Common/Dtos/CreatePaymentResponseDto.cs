namespace PaymentGateway.Common.Dtos
{
    public class CreatePaymentResponseDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string[] Errors { get; set; }
    }
}
