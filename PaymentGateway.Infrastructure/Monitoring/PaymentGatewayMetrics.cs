using PaymentGateway.Infrastructure.Monitoring.Errors;
using PaymentGateway.Infrastructure.Monitoring.Requests;
using Prometheus;

namespace PaymentGateway.Infrastructure.Monitoring
{
    public static class PaymentGatewayMetrics
    {

        public static Counter RequestCounter { get; } = Metrics.CreateCounter(
            "PaymentGatewayRequests",
            "Requests received",
            new CounterConfiguration
            {
                LabelNames = RequestLabels.LabelNames
            });

        public static Counter ErrorsCounter { get; } = Metrics.CreateCounter(
            "PaymentGatewayerrors",
            "Errors raised",
            new CounterConfiguration
            {
                LabelNames = ErrorLabels.LabelNames
            });
    }
}
