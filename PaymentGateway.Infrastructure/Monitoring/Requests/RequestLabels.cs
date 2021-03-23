using PaymentGateway.Common.RequestEnums;

namespace PaymentGateway.Infrastructure.Monitoring.Requests
{
    public static class RequestLabels
    {
        public static string[] LabelNames => new[] { "requestType"};
        public static string[] Labels(RequestType requestType)
        {
            return new[] { requestType.Label() };
        }
    }
}
