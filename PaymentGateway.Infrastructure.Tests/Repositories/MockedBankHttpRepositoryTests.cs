using Xunit;
using FluentAssertions;
using AutoFixture;
using PaymentGateway.Common.Domain;
using PaymentGateway.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure.Tests.Repositories
{
    public class MockedBankHttpRepositoryTests
    {

        [Fact]
        public async Task CreatePayment_ReturnsSuccess()
        {
            //Arrange
            var createPaymentRequest = new CreatePaymentRequest()
            {
                DebugExpectSuccess = true
            };
            var sut = new MockedBankHttpRepository();

            //Act
            var result = await sut.CreatePaymentAsync(createPaymentRequest);

            //Assert
            result.Should().BeOfType<CreatePaymentResponse>();
            result.Success.Should().BeTrue();
            result.TransactionId.Should().NotBeNullOrEmpty();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task CreatePayment_ReturnsFailure()
        {
            //Arrange
            var createPaymentRequest = new CreatePaymentRequest()
            {
                DebugExpectSuccess = false
            };
            var sut = new MockedBankHttpRepository();

            //Act
            var result = await sut.CreatePaymentAsync(createPaymentRequest);

            //Assert
            result.Should().BeOfType<CreatePaymentResponse>();
            result.Success.Should().BeFalse();
            result.TransactionId.Should().NotBeNullOrEmpty();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(PaymentStatusCode.NotSet, PaymentStatusCode.Success)]
        [InlineData(PaymentStatusCode.Success, PaymentStatusCode.Success)]
        [InlineData(PaymentStatusCode.Failure, PaymentStatusCode.Failure)]
        public async Task RetrievePaymentDetails_DetailsReturned(PaymentStatusCode expectedStatusCode, PaymentStatusCode actualStatusCode)
        {
            //Arrange
            var paymentDetailsRequest = new PaymentDetailsRequest()
            {
                DebugExpectedStatusCode = expectedStatusCode
            };
            var sut = new MockedBankHttpRepository();

            //Act
            var result = await sut.RetrievePaymentDetailsAsync(paymentDetailsRequest);

            //Assert
            result.Should().BeOfType<PaymentDetailsResponse>();
            result.CardNumber.Should().HaveLength(16);
            result.StatusCode.Should().Be(actualStatusCode);
        }

        [Fact]
        public async Task RetrievePaymentDetails_TransactionIdNotFound()
        {
            //Arrange
            var paymentDetailsRequest = new PaymentDetailsRequest()
            {
                DebugExpectedStatusCode = PaymentStatusCode.TransactionIdNotRecognised
            };
            var sut = new MockedBankHttpRepository();

            //Act
            var result = await sut.RetrievePaymentDetailsAsync(paymentDetailsRequest);

            //Assert
            result.Should().BeOfType<PaymentDetailsResponse>();
            result.CardNumber.Should().BeNull();
            result.StatusCode.Should().Be(PaymentStatusCode.TransactionIdNotRecognised);
        }
    }
}
