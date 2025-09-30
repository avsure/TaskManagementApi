using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Helpers
{
    public class FutureDateAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public FutureDateAttribute(string comaprisonProperty)
        {
            _comparisonProperty = comaprisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Unknown property:{_comparisonProperty}");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage ?? "Due date must be after created date");
            return ValidationResult.Success;
        }
    }
}
