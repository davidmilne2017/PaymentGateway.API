
namespace PaymentGateway.Common.Domain
{
    public class CreatePaymentResponse
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string[] Errors { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CreatePaymentResponse @object)
                return Success.Equals(@object.Success)
                    && TransactionId.Equals(@object.TransactionId)
                    && Errors.Equals(@object.Errors);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Success.GetHashCode();
                hash = hash * 23 + TransactionId.GetHashCode();
                hash = hash * 23 + Errors.GetHashCode();
                return hash;
            }
        }

    }
}
