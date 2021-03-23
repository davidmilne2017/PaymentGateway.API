using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

namespace PaymentGateway.API.Errors
{
    public class ErrorMessageBuilder
    {
        private readonly ErrorMessage errorMessage;

        private ErrorMessageBuilder(int statusCode, string errorTitle)
        {
            errorMessage = new ErrorMessage(statusCode, errorTitle);
        }

        public ErrorMessageBuilder AddErrorMessage(string errorKey, string message)
        {
            if (errorMessage.Errors.ContainsKey(errorKey))
            {
                var errors = errorMessage.Errors[errorKey];
                if (!errors.Contains(message))
                {
                    errors.Add(message);
                }
            }
            else
            {
                errorMessage.Errors.Add(errorKey, new List<string> { message });
            }
            return this;
        }

        public ErrorMessageBuilder AddErrorMessage(string errorKey, string[] messages)
        {
            foreach (var message in messages)
            {
                if (errorMessage.Errors.ContainsKey(errorKey))
                {
                    var errors = errorMessage.Errors[errorKey];
                    if (!errors.Contains(message))
                    {
                        errors.Add(message);
                    }
                }
                else
                {
                    errorMessage.Errors.Add(errorKey, new List<string> { message });
                }
            }

            return this;
        }

        public ErrorMessageBuilder AddErrorMessage(string errorKey, string format, params object[] args)
        {
            return AddErrorMessage(errorKey, string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public ErrorMessage Build()
        {
            return errorMessage;
        }

        public static ErrorMessageBuilder BadRequest()
        {
            return new ErrorMessageBuilder(StatusCodes.Status400BadRequest, ErrorMessageTitles.BadRequest);
        }

        public static ErrorMessageBuilder InternalServerError()
        {
            return new ErrorMessageBuilder(StatusCodes.Status500InternalServerError, ErrorMessageTitles.InternalServer);
        }

        public static ErrorMessageBuilder NotFound()
        {
            return new ErrorMessageBuilder(StatusCodes.Status404NotFound, ErrorMessageTitles.NotFound);
        }
    }
}
