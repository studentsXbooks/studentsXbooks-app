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

        [Theory]
        [InlineData("978-0-306-406157")]
        [InlineData("9780132350884")]
        [InlineData("978-0393912692")]
        [InlineData("9781974305032")]
        [InlineData("978-0-1359-5705-9")]
        public void isValidISBN13_ValidString_ShouldReturnTrue(string validISBN)
        {
            var result = ISBNChecker.isISBN13(validISBN);
            Assert.True(result);
        }


        [Theory]
        [InlineData("0")]
        [InlineData("01111111111111111111")]
        [InlineData("1234567890123")]
        public void isValidISBN13_InValidString_ShouldReturnFalse(string invalidISBN)
        {
            var result = ISBNChecker.isISBN13(invalidISBN);
            Assert.False(result);
        }
    }
}