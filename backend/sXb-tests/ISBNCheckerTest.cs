using sXb_service;
using Xunit;

namespace sXb_tests
{
    public class ISBNCheckerTest
    {
        [Theory]
        [InlineData("0802122280")]
        [InlineData("1617290653")]
        [InlineData("0981531687")]
        [InlineData("0-306-40615-2")]
        [InlineData("0-04-313341-X")]
        public void isValidISBN10_ValidString_ShouldReturnTrue(string validISBN)
        {
            var result = ISBNChecker.isISBN10(validISBN);
            Assert.True(result);
        }


        [Theory]
        [InlineData("0")]
        [InlineData("01111111111111111111")]
        [InlineData("1234567890")]
        public void isValidISBN10_InValidString_ShouldReturnFalse(string invalidISBN)
        {
            var result = ISBNChecker.isISBN10(invalidISBN);
            Assert.False(result);
        }
    }
}