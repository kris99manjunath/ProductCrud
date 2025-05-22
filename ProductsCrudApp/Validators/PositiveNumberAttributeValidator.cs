using System.ComponentModel.DataAnnotations;

namespace ProductsCrudApp.Validators
{
    public class PositiveNumberAttributeValidator : ValidationAttribute
    {
        public PositiveNumberAttributeValidator() : base("The value must be greater than 0.")
        {
        }

        public override bool IsValid(object value)
        {
            if (value is null) return true;

            return value switch
            {
                int intValue => intValue > 0,
                decimal decimalValue => decimalValue > 0,
                double doubleValue => doubleValue > 0,
                _ => false
            };
        }
    }

}

