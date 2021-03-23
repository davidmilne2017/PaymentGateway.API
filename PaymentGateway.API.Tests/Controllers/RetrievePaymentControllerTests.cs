using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using System.Threading.Tasks;
using PaymentGateway.Common.Interfaces;
using PaymentGateway.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Errors;
using System.Net;
using System.Linq;
using PaymentGateway.API.Resources;
using PaymentGateway.Common.Dtos;
using PaymentGateway.Common.Domain;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.API.Tests.Controllers
{
    public class RetrievePaymentControllerTests
    {

        private readonly Fixture fixture;

        public RetrievePaymentControllerTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public async Task RetrievePayment_WithNullBody_ReturnsBadRequest()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<RetrievePaymentController>>();
            var sut = new RetrievePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var error = ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.BodyMissing).Build();

            //Act
            var response = await sut.RetrievePaymentDetails(null);

            //Assert
            var result = response.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var value = result.Value.Should().BeOfType<ErrorMessage>().Subject;
            value.Status.Should().Be((int)HttpStatusCode.BadRequest);
            value.Title.Should().Be(ErrorMessageTitles.BadRequest);
            value.Errors.Should().HaveCount(1);
            value.Errors.First().Value.Should().HaveCount(1);
            value.Errors.First().Value.First().Should().Be(error.Errors.First().Value.First());
        }

        [Fact]
        public async Task CreatePayment_WithValidationErrors_ReturnsBadRequest()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<RetrievePaymentController>>();
            var sut = new RetrievePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<PaymentDetailsRequestDto>();
            create.TransactionId = "";
            var error = ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.NoTransactionId).Build();

            //Act
            var response = await sut.RetrievePaymentDetails(create);

            //Assert
            var result = response.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var value = result.Value.Should().BeOfType<ErrorMessage>().Subject;
            value.Status.Should().Be((int)HttpStatusCode.BadRequest);
            value.Title.Should().Be(ErrorMessageTitles.BadRequest);
            value.Errors.Should().HaveCount(1);
            value.Errors.First().Value.Should().HaveCount(1);
            value.Errors.First().Value.First().Should().Be(error.Errors.First().Value.First());
        }

        [Fact]
        public async Task CreatePayment_NullReturn_ReturnsInternalServerError()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<RetrievePaymentController>>();
            var sut = new RetrievePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<PaymentDetailsRequestDto>();
            paymentServiceMock.Setup(x => x.RetrievePaymentDetailsAsync(It.IsAny<PaymentDetailsRequest>())).ReturnsAsync((PaymentDetailsResponse)null);
            var error = ErrorMessageBuilder.InternalServerError().AddErrorMessage("payment", ErrorMessages.CouldNotRetrievePaymentDetails).Build();

            //Act
            var response = await sut.RetrievePaymentDetails(create);

            //Assert
            var result = response.Result.Should().BeOfType<ObjectResult>().Subject;
            var value = result.Value.Should().BeOfType<ErrorMessage>().Subject;
            value.Status.Should().Be((int)HttpStatusCode.InternalServerError);
            value.Title.Should().Be(ErrorMessageTitles.InternalServer);
            value.Errors.Should().HaveCount(1);
            value.Errors.First().Value.Should().HaveCount(1);
            value.Errors.First().Value.First().Should().Be(error.Errors.First().Value.First());
        }

        [Fact]
        public async Task CreatePayment_StatusCodeTransactionIdNotRecognised_ReturnsNotFoundError()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<RetrievePaymentController>>();
            var sut = new RetrievePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<PaymentDetailsRequestDto>();
            var paymentResponse = fixture.Create<PaymentDetailsResponse>();
            paymentResponse.StatusCode = PaymentStatusCode.TransactionIdNotRecognised;
            paymentServiceMock.Setup(x => x.RetrievePaymentDetailsAsync(It.IsAny<PaymentDetailsRequest>())).ReturnsAsync(paymentResponse);
            var error = ErrorMessageBuilder.InternalServerError().AddErrorMessage("transactionId", ErrorMessages.TransactionIdNotFound, create.TransactionId).Build();

            //Act
            var response = await sut.RetrievePaymentDetails(create);

            //Assert
            var result = response.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var value = result.Value.Should().BeOfType<ErrorMessage>().Subject;
            value.Status.Should().Be((int)HttpStatusCode.NotFound);
            value.Title.Should().Be(ErrorMessageTitles.NotFound);
            value.Errors.Should().HaveCount(1);
            value.Errors.First().Value.Should().HaveCount(1);
            value.Errors.First().Value.First().Should().Be(error.Errors.First().Value.First());
        }

        [Fact]
        public async Task CreatePayment_ObjectCreated_ReturnsCreatedObject()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<RetrievePaymentController>>();
            var sut = new RetrievePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<PaymentDetailsRequestDto>();
            var paymentResponse = fixture.Create<PaymentDetailsResponse>();
            paymentResponse.StatusCode = PaymentStatusCode.Success;
            paymentServiceMock.Setup(x => x.RetrievePaymentDetailsAsync(It.IsAny<PaymentDetailsRequest>())).ReturnsAsync(paymentResponse);

            //Act
            var response = await sut.RetrievePaymentDetails(create);

            //Assert
            var result = response.Result.Should().BeOfType<OkObjectResult>().Subject;
            _ = result.Value.Should().BeOfType<PaymentDetailsResponseDto>().Subject;
        }
    }
}
