using Microsoft.Extensions.Logging;
using PaymentGateway.Common.ErrorEnums;
using System;

namespace PaymentGateway.Infrastructure.Monitoring.Errors
{
    public static class ErrorExtensions
    {
        public static string Label(this ErrorCategory value)
            => value.ToString().ToLower();

        public static string Label(this ErrorLevel value)
            => value.ToString().ToLower();
        

        public static void CustomLogError(this ILogger logger, ErrorCategory category, string message)
        {
            PaymentGatewayMetrics.ErrorsCounter.WithLabels(ErrorLabels.Labels(category, ErrorLevel.ERROR)).Inc();
            logger.LogError(message);
        }

        public static void CustomLogError(this ILogger logger, ErrorCategory category, Exception exception, string message)
        {
            PaymentGatewayMetrics.ErrorsCounter.WithLabels(ErrorLabels.Labels(category, ErrorLevel.ERROR)).Inc();
            logger.LogError(exception, message);
        }

        public static void CustomLogWarning(this ILogger logger, ErrorCategory category, string message)
        {
            PaymentGatewayMetrics.ErrorsCounter.WithLabels(ErrorLabels.Labels(category, ErrorLevel.WARNING)).Inc();
            logger.LogError(message);
        }

    }
}