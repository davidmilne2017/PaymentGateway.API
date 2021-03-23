using System.Collections.Generic;

namespace PaymentGateway.API.Errors
{
    public class ErrorMessage
    {
        public ErrorMessage(int status, string title)
        {
            Status = status;
            Title = title;
            Errors = new Dictionary<string, List<string>>();
        }

        public string Title { get; private set; }

        public int Status { get; private set; }

        public Dictionary<string, List<string>> Errors { get; private set; }
    }
}
