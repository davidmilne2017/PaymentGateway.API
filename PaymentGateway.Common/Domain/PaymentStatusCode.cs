namespace PaymentGateway.Common.Domain
{
    public enum PaymentStatusCode
    {
        NotSet = 0,
        Success = 1,
        Failure = 2,
        TransactionIdNotRecognised = 3
    }
}
