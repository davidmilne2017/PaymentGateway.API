using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Interfaces;
using PaymentGateway.Services.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Services.Tests.Services
{
    public class PaymentServiceTests
    {

        private readonly Fixture fixture;
        
        public PaymentServiceTests()
        {
            fixture = new Fixture();
        }
        
        [Fact]
        public void ValidateCreatePaymentRequest_AddsFirstConditionalErrors_Correctly()
        {
            //Arrange
            var paymentRequest = fixture.Create<CreatePaymentRequest>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            paymentValidatorsMock.Setup(x => x.ValidateCardHolderName(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateCardNumber(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryMonth(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryYear(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateAmount(It.IsAny<decimal>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateCurrencyCode(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateCvv(It.IsAny<string>())).Returns(false);

            //Act
            var result = sut.ValidateCreatePaymentRequest(paymentRequest);

            //Assert
            result.Should().BeOfType<string[]>();
            result.Should().HaveCount(7);
            result[0].Should().Be(PaymentService.InvalidCardHolderNameError);
            result[1].Should().Be(PaymentService.InvalidCardNumberError);
            result[2].Should().Be(PaymentService.InvalidExpiryMonth);
            result[3].Should().Be(PaymentService.InvalidExpiryYear);
            result[4].Should().Be(PaymentService.InvalidAmount);
            result[5].Should().Be(PaymentService.InvalidCurrencyCode);
            result[6].Should().Be(PaymentService.InvalidCvv);
        }

        [Fact]
        public void ValidateCreatePaymentRequest_AddsSecondConditionalErrors_Correctly()
        {
            //Arrange
            var paymentRequest = fixture.Create<CreatePaymentRequest>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            paymentValidatorsMock.Setup(x => x.ValidateCardHolderName(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCardNumber(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.LuhnCheck(It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryMonth(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryYear(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryDate(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            paymentValidatorsMock.Setup(x => x.ValidateAmount(It.IsAny<decimal>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCurrencyCode(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCvv(It.IsAny<string>())).Returns(true);

            //Act
            var result = sut.ValidateCreatePaymentRequest(paymentRequest);

            //Assert
            result.Should().BeOfType<string[]>();
            result.Should().HaveCount(2);
            result.Should().Contain(PaymentService.InvalidCardNumberError);
            result.Should().Contain(PaymentService.CardExpired);
        }

        [Fact]
        public void ValidateCreatePaymentRequest_NoErrors_ReturnsEmptyErrors()
        {
            //Arrange
            var paymentRequest = fixture.Create<CreatePaymentRequest>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            paymentValidatorsMock.Setup(x => x.ValidateCardHolderName(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCardNumber(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.LuhnCheck(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryMonth(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryYear(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateExpiryDate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateAmount(It.IsAny<decimal>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCurrencyCode(It.IsAny<string>())).Returns(true);
            paymentValidatorsMock.Setup(x => x.ValidateCvv(It.IsAny<string>())).Returns(true);

            //Act
            var result = sut.ValidateCreatePaymentRequest(paymentRequest);

            //Assert
            result.Should().BeOfType<string[]>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreatePaymentAsync_ThrowsException_ReturnsNull()
        {
            //Arrange
            var paymentRequest = fixture.Create<CreatePaymentRequest>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            bankHttpRepositoryMock.Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>())).Throws(new System.Exception());

            //Act
            var result = await sut.CreatePaymentAsync(paymentRequest);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreatePaymentAsync_ReturnsValidResponse()
        {
            //Arrange
            var paymentRequest = fixture.Create<CreatePaymentRequest>();
            var paymentResponse = fixture.Create<CreatePaymentResponse>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            bankHttpRepositoryMock.Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>())).ReturnsAsync(paymentResponse);

            //Act
            var result = await sut.CreatePaymentAsync(paymentRequest);

            //Assert
            result.Should().BeOfType<CreatePaymentResponse>();
            result.Should().Be(paymentResponse);
        }

        [Fact]
        public async Task RetrievePaymentAsync_ThrowsException_ReturnsNull()
        {
            //Arrange
            var paymentDetailsRequest = fixture.Create<PaymentDetailsRequest>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            bankHttpRepositoryMock.Setup(x => x.RetrievePaymentDetailsAsync(It.IsAny<PaymentDetailsRequest>())).Throws(new System.Exception());

            //Act
            var result = await sut.RetrievePaymentDetailsAsync(paymentDetailsRequest);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RetrievePaymentAsync_ReturnsValidResponse()
        {
            //Arrange
            var paymentRequest = fixture.Create<PaymentDetailsRequest>();
            var paymentResponse = fixture.Create<PaymentDetailsResponse>();
            var bankHttpRepositoryMock = new Mock<IBankHttpRepository>();
            var paymentValidatorsMock = new Mock<IPaymentValidators>();
            var loggerMock = new Mock<ILogger<PaymentService>>();
            var sut = new PaymentService(bankHttpRepositoryMock.Object, paymentValidatorsMock.Object, loggerMock.Object);
            bankHttpRepositoryMock.Setup(x => x.RetrievePaymentDetailsAsync(It.IsAny<PaymentDetailsRequest>())).ReturnsAsync(paymentResponse);

            //Act
            var result = await sut.RetrievePaymentDetailsAsync(paymentRequest);

            //Assert
            result.Should().BeOfType<PaymentDetailsResponse>();
            result.Should().Be(result);
        }

    }
}
