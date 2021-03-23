using Xunit;
using FluentAssertions;
using PaymentGateway.Services.Services;
using System;

namespace PaymentGateway.Services.Tests.Services
{
    public class PaymentValidatorsTests
    {

        [Theory]
        [InlineData((string)null, false)]
        [InlineData("", false)]
        [InlineData("string", true)]        
        public void CardHolderNameCheck(string cardHolderName, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateCardHolderName(cardHolderName);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData((string)null, false)]
        [InlineData("", false)]
        [InlineData("-1", false)]
        [InlineData("1234", true)]
        public void CardNumberCheck(string cardNumber, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateCardNumber(cardNumber);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("",false)]
        [InlineData("string", false)]
        [InlineData("1234", false)]
        [InlineData("4444444444444448", true)]
        public void LuhnCheck(string cardNumber, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.LuhnCheck(cardNumber);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("string", false)]
        [InlineData("0", false)]
        [InlineData("123", false)]
        [InlineData("1", true)]
        [InlineData("01", true)]
        [InlineData("12", true)]
        [InlineData("13", false)]
        public void ValidateExpiryMonth(string expiryMonth, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryMonth(expiryMonth);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("string", false)]
        [InlineData("0", false)]
        [InlineData("35", false)]
        public void ValidateExpiryYear_WithInvalidFormats(string expiryYear, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryYear(expiryYear);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void ValidateExpiryYear_WithValidFormats_ValidYear()
        {
            //Arrange
            var expiryYear = DateTime.Now.Year.ToString();
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryYear(expiryYear);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateExpiryYear_WithValidFormats_InvalidYear()
        {
            //Arrange
            var expiryYear = (DateTime.Now.Year - 1).ToString();
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryYear(expiryYear);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateExpiryDate_WithSameMonth_DateInThePast()
        {
            //Arrange
            var expiryYear = (DateTime.Now.Year).ToString();
            var expiryMonth = (DateTime.Now.Month).ToString();
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryDate(expiryMonth, expiryYear);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateExpiryDate_WithDateInTheFuture()
        {
            //Arrange
            var expiryYear = (DateTime.Now.Year + 1).ToString();
            var expiryMonth = (DateTime.Now.Month).ToString();
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateExpiryDate(expiryMonth, expiryYear);

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(75.99, true)]
        public void ValidateAmount(decimal amount, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateAmount(amount);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("XX", false)]
        [InlineData("XXX", true)]
        [InlineData("XXXX", false)]
        public void ValidateCurrencyCode(string currencyCode, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateCurrencyCode(currencyCode);

            //Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("string", false)]
        [InlineData("12", false)]
        [InlineData("456", true)]
        [InlineData("4568", true)]
        [InlineData("45689", false)]
        public void ValidateCvvNumber(string cvv, bool expected)
        {
            //Arrange
            var sut = new PaymentValidators();

            //Act
            var result = sut.ValidateCvv(cvv);

            //Assert
            result.Should().Be(expected);
        }

    }
}
