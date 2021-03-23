namespace PaymentGateway.Common.Domain
{
    public class PaymentDetailsResponse
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public PaymentStatusCode StatusCode { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PaymentDetailsResponse @object)
                return CardHolderName.Equals(@object.CardHolderName)
                    && CardNumber.Equals(@object.CardNumber)
                    && ExpiryMonth.Equals(@object.ExpiryMonth)
                    && ExpiryYear.Equals(@object.ExpiryYear)
                    && Amount.Equals(@object.Amount)
                    && CurrencyCode.Equals(@object.CurrencyCode)
                    && StatusCode.Equals(@object.StatusCode);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + CardHolderName.GetHashCode();
                hash = hash * 23 + CardNumber.GetHashCode();
                hash = hash * 23 + ExpiryMonth.GetHashCode();
                hash = hash * 23 + ExpiryYear.GetHashCode();
                hash = hash * 23 + Amount.GetHashCode();
                hash = hash * 23 + CurrencyCode.GetHashCode();
                hash = hash * 23 + StatusCode.GetHashCode();
                return hash;
            }
        }
    }
}
