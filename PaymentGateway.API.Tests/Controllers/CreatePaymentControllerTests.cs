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
    public class CreatePaymentControllerTests
    {
        private readonly Fixture fixture;

        public CreatePaymentControllerTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public async Task CreatePayment_WithNullBody_ReturnsBadRequest()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<CreatePaymentController>>();
            var sut = new CreatePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var error = ErrorMessageBuilder.BadRequest().AddErrorMessage("body", ErrorMessages.BodyMissing).Build();

            //Act
            var response = await sut.CreatePayment(null);

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
            var loggerMock = new Mock<ILogger<CreatePaymentController>>();
            var sut = new CreatePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<CreatePaymentRequestDto>();
            var validationError = "Some validation error";
            paymentServiceMock.Setup(x => x.ValidateCreatePaymentRequest(It.IsAny<CreatePaymentRequest>())).Returns(new string[] { validationError });
            var error = ErrorMessageBuilder.BadRequest().AddErrorMessage("body", validationError).Build();
            
            //Act
            var response = await sut.CreatePayment(create);

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
            var loggerMock = new Mock<ILogger<CreatePaymentController>>();
            var sut = new CreatePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<CreatePaymentRequestDto>();
            paymentServiceMock.Setup(x => x.ValidateCreatePaymentRequest(It.IsAny<CreatePaymentRequest>())).Returns(new string[0] { });
            paymentServiceMock.Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>())).ReturnsAsync((CreatePaymentResponse)null);
            var error = ErrorMessageBuilder.InternalServerError().AddErrorMessage("payment", ErrorMessages.CouldNotCreatePayment).Build();

            //Act
            var response = await sut.CreatePayment(create);

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
        public async Task CreatePayment_ObjectCreated_ReturnsCreatedObject()
        {
            //Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            var loggerMock = new Mock<ILogger<CreatePaymentController>>();
            var sut = new CreatePaymentController(paymentServiceMock.Object, loggerMock.Object);
            var create = fixture.Create<CreatePaymentRequestDto>();
            var createResponse = fixture.Create<CreatePaymentResponse>();
            paymentServiceMock.Setup(x => x.ValidateCreatePaymentRequest(It.IsAny<CreatePaymentRequest>())).Returns(new string[0] { });
            paymentServiceMock.Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>())).ReturnsAsync(createResponse);
            
            //Act
            var response = await sut.CreatePayment(create);

            //Assert
            var result = response.Result.Should().BeOfType<OkObjectResult>().Subject;
            _ = result.Value.Should().BeOfType<CreatePaymentResponse>().Subject;
        }
    }
}
