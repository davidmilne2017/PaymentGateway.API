using System;

namespace PaymentGateway.Common.Domain
{
    public class PaymentDetailsRequest
    {
        public string TransactionId { get; set; }
        public PaymentStatusCode DebugExpectedStatusCode { get; set; }

    }
}
