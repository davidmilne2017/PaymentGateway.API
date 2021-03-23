using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Errors;
using PaymentGateway.API.Resources;
using PaymentGateway.Common.Dtos;
using PaymentGateway.Common.ErrorEnums;
using PaymentGateway.Common.Interfaces;
using PaymentGateway.Common.Mappers;
using PaymentGateway.Common.RequestEnums;
using PaymentGateway.Common.Resources;
using PaymentGateway.Infrastructure.Monitoring;
using PaymentGateway.Infrastructure.Monitoring.Errors;
using PaymentGateway.Infrastructure.Monitoring.Requests;
using System.Threading.Tasks;

namespace PaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class RetrievePaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<RetrievePaymentController> logger;

        public RetrievePaymentController(IPaymentService paymentService, ILogger<RetrievePaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets payment details by Transaction Id
        /// </summary>
        /// <param name="paymentDetailsRequestDto"></param>
        /// <returns>A list of Notifications for a userId</returns>
        /// <response code="200"></response>
        /// <response code="400">Returns a bad request error</response>
        /// <response code="404">Returns a not found error</response>
        /// <response code="500">Returns a not found error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentDetailsResponseDto>> RetrievePaymentDetails(PaymentDetailsRequestDto paymentDetailsRequestDto)
        {

            PaymentGatewayMetrics.RequestCounter.WithLabels(RequestType.RETRIEVEDETAILS.Label());

            if (paymentDetailsRequestDto == null)
            {
                logger.CustomLogError(ErrorCategory.BUSINESS, LoggerErrorMessages.NullRequestBody);
                return BadRequest(ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.BodyMissing).Build());
            }                

            if (string.IsNullOrEmpty(paymentDetailsRequestDto.TransactionId))
            {
                logger.CustomLogWarning(ErrorCategory.BUSINESS, LoggerErrorMessages.ValidationErrors);
                return BadRequest(ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.NoTransactionId).Build());
            }                

            var paymentDetailsRequest = paymentDetailsRequestDto.MapPaymentDetailsRequestDtoToPaymentDetailsRequest();

            var headers = HttpContext.Request.Headers;
            if (headers.ContainsKey("DebugExpectedStatus"))
            {
                var debugExpectedStatus = headers["DebugExpectedStatus"];
                if (!string.IsNullOrEmpty(debugExpectedStatus)) 
                    paymentDetailsRequest.SetDebugExpectedStatus(debugExpectedStatus);
            }

            var paymentDetails = await paymentService.RetrievePaymentDetailsAsync(paymentDetailsRequest).ConfigureAwait(false);

            if (paymentDetails == null)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, LoggerErrorMessages.CouldNotRetrievePayment);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageBuilder.InternalServerError().AddErrorMessage("payment",
                        ErrorMessages.CouldNotRetrievePaymentDetails).Build());
            }
                
            if (paymentDetails.StatusCode == Common.Domain.PaymentStatusCode.TransactionIdNotRecognised)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, LoggerErrorMessages.TransactionIdNotFound);
                return NotFound(ErrorMessageBuilder.NotFound().AddErrorMessage("transactionId", ErrorMessages.TransactionIdNotFound,
                    paymentDetailsRequestDto.TransactionId).Build());
            }

            return Ok(paymentDetails.MapPaymentDetailsResponseToPaymentDetailsResponseDto());
        }

    }
}
