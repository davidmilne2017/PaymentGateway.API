using PaymentGateway.Common.RequestEnums;

namespace PaymentGateway.Infrastructure.Monitoring.Requests
{
    public static class RequestTypeExtensions
    {
        public static string Label(this RequestType value)
           => value.ToString().ToLower();

    }
}
