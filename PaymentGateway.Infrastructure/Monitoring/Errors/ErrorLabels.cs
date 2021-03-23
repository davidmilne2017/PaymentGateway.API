using PaymentGateway.Common.ErrorEnums;

namespace PaymentGateway.Infrastructure.Monitoring.Errors
{
    public static class ErrorLabels
    {
        public static string[] LabelNames => new[] { "category", "level" };
        public static string[] Labels(ErrorCategory category, ErrorLevel level)
        {
            return new[] {category.Label(), level.Label() };
        }
        
    }
}
