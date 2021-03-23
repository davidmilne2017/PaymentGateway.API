using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Errors;
using PaymentGateway.API.Resources;
using PaymentGateway.Common.Dtos;
using PaymentGateway.Common.Interfaces;
using PaymentGateway.Common.Mappers;
using PaymentGateway.Common.RequestEnums;
using PaymentGateway.Common.ErrorEnums;
using PaymentGateway.Common.Resources;
using PaymentGateway.Infrastructure.Monitoring;
using PaymentGateway.Infrastructure.Monitoring.Requests;
using PaymentGateway.Infrastructure.Monitoring.Errors;
using System.Threading.Tasks;

namespace PaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CreatePaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<CreatePaymentController> logger;

        public CreatePaymentController(IPaymentService paymentService, ILogger<CreatePaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates Payment Record with Bank Accquirer
        /// </summary>
        /// <returns>Create Payment Response</returns>
        /// <response code="200"></response>
        /// <response code="400">Returns a bad request error</response>
        /// <response code="500">Returns an internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreatePaymentResponseDto>> CreatePayment(CreatePaymentRequestDto createPaymentRequestDto)
        {
            PaymentGatewayMetrics.RequestCounter.WithLabels(RequestType.CREATEPAYMENT.Label());

            if (createPaymentRequestDto == null)
            {
                logger.CustomLogError(ErrorCategory.BUSINESS, LoggerErrorMessages.NullRequestBody);
                return BadRequest(ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.BodyMissing).Build());
            }           

            var createPaymentRequest = createPaymentRequestDto.MapPaymentRequestDtoToPaymentRequest();

            var headers = HttpContext.Request.Headers;
            if (headers.ContainsKey("DebugExpectedSuccess") && bool.TryParse(headers["DebugExpectedSuccess"], out var debugExpectedSuccess))
               createPaymentRequest.DebugExpectSuccess = debugExpectedSuccess;
            

            var validationErrors = paymentService.ValidateCreatePaymentRequest(createPaymentRequest);
            if (validationErrors.Length > 0)
            {
                logger.CustomLogWarning(ErrorCategory.BUSINESS, LoggerErrorMessages.ValidationErrors);
                return BadRequest(ErrorMessageBuilder.BadRequest().AddErrorMessage("body", validationErrors).Build());
            }

            var created = await paymentService.CreatePaymentAsync(createPaymentRequest).ConfigureAwait(false);

            if (created == null)
            {
                logger.CustomLogError(ErrorCategory.APPLICATION, LoggerErrorMessages.CouldNotCreatePayment);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageBuilder.InternalServerError().AddErrorMessage("payment",
                        ErrorMessages.CouldNotCreatePayment).Build());
            } 
            
            return Ok(created);
            
        }
    }
}
