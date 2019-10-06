using System;
using System.ComponentModel.DataAnnotations;

namespace sXb_service.Helpers.ModelValidation
{
    public class ISBNValidationAttribute : ValidationAttribute
    {
        public ISBNValidationAttribute()
        {
        }

        public ISBNValidationAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        public ISBNValidationAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public ISBNTypes ISBNType { get; set; }

        public override bool IsValid(object value)
        {
            if (value is string s)
            {
                switch (this.ISBNType)
                {
                    case ISBNTypes.ISBN10:
                        return ISBNChecker.isISBN10(s);
                    case ISBNTypes.ISBN13:
                        return ISBNChecker.isISBN13(s);
                }
            }
            return false;
        }
    }
    public enum ISBNTypes
    {
        ISBN10, ISBN13
    }
}