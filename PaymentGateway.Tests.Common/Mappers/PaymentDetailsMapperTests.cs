using Xunit;
using FluentAssertions;
using AutoFixture;
using PaymentGateway.Common.Dtos;
using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Mappers;

namespace PaymentGateway.Common.Tests.Mappers
{
    public class PaymentDetailsMapperTests
    {
        private readonly Fixture fixture;

        public PaymentDetailsMapperTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void MapPaymentDetailsRequestDtoToPaymentDetailsRequest_MapsObjectCorrectly()
        {
            //Arrange
            var requestDto = fixture.Create<PaymentDetailsRequestDto>();

            //Act
            var result = requestDto.MapPaymentDetailsRequestDtoToPaymentDetailsRequest();

            //Assert
            result.Should().BeOfType<PaymentDetailsRequest>();
            result.TransactionId.Should().Be(requestDto.TransactionId);
        }

        [Fact]
        public void MapPaymentDetailsResponseToPaymentDetailsResponseDto_MapsObjectCorrectly()
        {
            //Arrange
            var response = fixture.Create<PaymentDetailsResponse>();

            //Act
            var result = response.MapPaymentDetailsResponseToPaymentDetailsResponseDto();

            //Assert
            result.Should().BeOfType<PaymentDetailsResponseDto>();
            result.CardHolderName.Should().Be(response.CardHolderName);
            result.CardNumber.Should().Be(response.CardNumber);
            result.ExpiryMonth.Should().Be(response.ExpiryMonth);
            result.ExpiryYear.Should().Be(response.ExpiryYear);
            result.Amount.Should().Be(response.Amount);
            result.CurrencyCode.Should().Be(response.CurrencyCode);
            result.StatusCode.Should().Be(response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotSet", PaymentStatusCode.NotSet)]
        [InlineData("Success", PaymentStatusCode.Success)]
        [InlineData("Failure", PaymentStatusCode.Failure)]
        [InlineData("TransactionIdNotRecognised", PaymentStatusCode.TransactionIdNotRecognised)]
        [InlineData("InvalidEnum", PaymentStatusCode.NotSet)]
        [InlineData("", PaymentStatusCode.NotSet)]
        public void SetDebugExpectedStatus_SetsCorrectValue(string debugExpectedStatus, PaymentStatusCode debugActualStatus)
        {
            //Arrange
            var sut = new PaymentDetailsRequest();

            //Act
            sut.SetDebugExpectedStatus(debugExpectedStatus);

            //Assert
            sut.DebugExpectedStatusCode.Should().Be(debugActualStatus);
        }
    }
}
