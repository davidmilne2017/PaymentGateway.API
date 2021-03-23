namespace PaymentGateway.Common.Interfaces
{
    public interface IPaymentValidators
    {
        bool ValidateCardHolderName(string cardHolderName);
        bool ValidateCardNumber(string cardNumber);
        bool LuhnCheck(string cardNumber);
        bool ValidateExpiryMonth(string expiryMonth);
        bool ValidateExpiryYear(string expiryYear);
        bool ValidateExpiryDate(string expiryMonth, string expiryYear);
        bool ValidateAmount(decimal amount);
        bool ValidateCurrencyCode(string currencyCode);
        bool ValidateCvv(string cvv);
    }
}
