using Microsoft.Extensions.Logging;
using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentGateway.Infrastructure.Monitoring.Errors;
using PaymentGateway.Common.ErrorEnums;
using PaymentGateway.Common.Resources;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IBankHttpRepository bankHttpRepository;
        private readonly IPaymentValidators paymentValidators;
        private readonly ILogger<PaymentService> logger;

        public const string InvalidCardHolderNameError = "A valid cardholdername must be specified";
        public const string InvalidCardNumberError = "A valid numeric cardnumber must be specified";
        public const string InvalidExpiryMonth = "A valid numeric expirymonth in the range 1-12 must be specified";
        public const string InvalidExpiryYear = "A valid numeric expirytear in the range 1-12 must be specified";
        public const string CardExpired = "Expiry dates must be in the future";
        public const string InvalidAmount = "A valid amount must be specified";
        public const string InvalidCurrencyCode = "A valid currencycode with 3 characters must be specified";
        public const string InvalidCvv = "A valid cvv number with 3-4 digits must be specified";

        public PaymentService(IBankHttpRepository bankHttpRepository, IPaymentValidators paymentValidators, ILogger<PaymentService> logger)
        {
            this.bankHttpRepository = bankHttpRepository;
            this.paymentValidators = paymentValidators;
            this.logger = logger;
        }

        public string[] ValidateCreatePaymentRequest(CreatePaymentRequest createPaymentRequest)
        {
            var errors = new List<string>();
            try
            {
                if (!paymentValidators.ValidateCardHolderName(createPaymentRequest.CardHolderName))
                    errors.Add(InvalidCardHolderNameError);

                if (!paymentValidators.ValidateCardNumber(createPaymentRequest.CardNumber) || !paymentValidators.LuhnCheck(createPaymentRequest.CardNumber))
                    errors.Add(InvalidCardNumberError);

                if (!paymentValidators.ValidateExpiryMonth(createPaymentRequest.ExpiryMonth))
                    errors.Add(InvalidExpiryMonth);

                if (!paymentValidators.ValidateExpiryYear(createPaymentRequest.ExpiryYear))
                    errors.Add(InvalidExpiryYear);

                if (!errors.Contains(InvalidExpiryMonth) && !errors.Contains(InvalidExpiryYear)
                        && !paymentValidators.ValidateExpiryDate(createPaymentRequest.ExpiryMonth, createPaymentRequest.ExpiryYear))
                    errors.Add(CardExpired);

                if (!paymentValidators.ValidateAmount(createPaymentRequest.Amount))
                    errors.Add(InvalidAmount);

                if (!paymentValidators.ValidateCurrencyCode(createPaymentRequest.CurrencyCode))
                    errors.Add(InvalidCurrencyCode);

                if (!paymentValidators.ValidateCvv(createPaymentRequest.Cvv))
                    errors.Add(InvalidCvv);
            }
            catch (Exception ex)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, ex, LoggerErrorMessages.PaymentValidatorsException);
            }
            
            return errors.ToArray();
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest createPaymentRequest)
        {
            try
            {
                return await bankHttpRepository.CreatePaymentAsync(createPaymentRequest).ConfigureAwait(false); 
            } catch (Exception ex)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, ex, LoggerErrorMessages.CreatePaymentRepositoryException);
                return null;
            }
        }

        public async Task<PaymentDetailsResponse> RetrievePaymentDetailsAsync(PaymentDetailsRequest paymentDetailsRequest)
        {
            try
            {
                return await bankHttpRepository.RetrievePaymentDetailsAsync(paymentDetailsRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, ex, LoggerErrorMessages.RetrievePaymentDetailsRepositoryException);
                return null;
            }
        }
    }
}
