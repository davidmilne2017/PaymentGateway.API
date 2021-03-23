using PaymentGateway.Common.Interfaces;
using System;

namespace PaymentGateway.Services.Services
{
    public class PaymentValidators : IPaymentValidators
    {
        public bool ValidateCardHolderName(string cardHolderName)
         => !string.IsNullOrEmpty(cardHolderName);

        public bool ValidateCardNumber(string cardNumber)
         => !string.IsNullOrEmpty(cardNumber) && ulong.TryParse(cardNumber, out var _);

        public bool LuhnCheck(string cardNumber)
        {

            if (!ulong.TryParse(cardNumber, out var _))
                return false;

            int nDigits = cardNumber.Length;

            int nSum = 0;
            bool isSecond = false;
            for (int i = nDigits - 1; i >= 0; i--)
            {

                int d = cardNumber[i] - '0';

                if (isSecond)
                    d = d * 2;

                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);
        }

        public bool ValidateExpiryMonth(string expiryMonth)
        {
            if (string.IsNullOrEmpty(expiryMonth))
                return false;

            if (expiryMonth.Length > 2)
                return false;

            uint numericExpiryMonth;
            if (!uint.TryParse(expiryMonth, out numericExpiryMonth))
                return false;

            if (numericExpiryMonth == 0 || numericExpiryMonth > 12)
                return false;

            return true;
        }

        public bool ValidateExpiryYear(string expiryYear)
        {
            if (string.IsNullOrEmpty(expiryYear))
                return false;

            if (expiryYear.Length != 4)
                return false;

            uint numericExpiryYear;
            if (!uint.TryParse(expiryYear, out numericExpiryYear))
                return false;

            if (numericExpiryYear < DateTime.Now.Year)
                return false;

            return true;
        }

        public bool ValidateExpiryDate(string expiryMonth, string expiryYear)
        {
            if (!ValidateExpiryMonth(expiryMonth) || !ValidateExpiryYear(expiryYear))
                return false;

            var expiryDate = new DateTime(int.Parse(expiryYear), int.Parse(expiryMonth), 1);

            if (expiryDate < DateTime.Now)
                return false;

            return true;
        }

        public bool ValidateAmount(decimal amount)
            => amount > 0;

        public bool ValidateCurrencyCode(string currencyCode)
            => currencyCode.Length == 3;

        public bool ValidateCvv(string cvv)
        {
            if (cvv.Length < 3 || cvv.Length > 4)
                return false;

            if (!uint.TryParse(cvv, out var _))
                return false;

            return true;
        }
    }
}
