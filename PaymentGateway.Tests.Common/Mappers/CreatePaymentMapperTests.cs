using Xunit;
using FluentAssertions;
using AutoFixture;
using PaymentGateway.Common.Dtos;
using PaymentGateway.Common.Domain;
using PaymentGateway.Common.Mappers;

namespace PaymentGateway.Common.Tests.Mappers
{
    public class CreatePaymentMapperTests
    {
        private readonly Fixture fixture;

        public CreatePaymentMapperTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void MapPaymentRequestDtoToPaymentRequest_MapsObjectCorrectly()
        {
            //Arrange
            var requestDto = fixture.Create<CreatePaymentRequestDto>();

            //Act
            var result = requestDto.MapPaymentRequestDtoToPaymentRequest();

            //Assert
            result.Should().BeOfType<CreatePaymentRequest>();
            result.CardHolderName.Should().Be(requestDto.CardHolderName);
            result.CardNumber.Should().Be(requestDto.CardNumber);
            result.ExpiryMonth.Should().Be(requestDto.ExpiryMonth);
            result.ExpiryYear.Should().Be(requestDto.ExpiryYear);
            result.Amount.Should().Be(requestDto.Amount);
            result.CurrencyCode.Should().Be(requestDto.CurrencyCode);
            result.Cvv.Should().Be(requestDto.CVV);
        }

        [Fact]
        public void MapPaymentResponseToPaymentResponseDto_MapsObjectCorrectly()
        {
            //Arrange
            var response = fixture.Create<CreatePaymentResponse>();

            //Act
            var result = response.MapPaymentResponseToPaymentResponseDto();

            //Assert
            result.Should().BeOfType<CreatePaymentResponseDto>();
            result.Success.Should().Be(response.Success);
            result.TransactionId.Should().Be(response.TransactionId);
            result.Errors.Should().HaveCount(response.Errors.Length);
            result.Errors[0].Should().Be(response.Errors[0]);
        }

    }
}
