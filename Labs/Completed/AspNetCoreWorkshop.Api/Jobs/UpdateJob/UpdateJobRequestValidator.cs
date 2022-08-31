using System;
using System.Linq;
using System.Text.Json;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using FluentValidation;
using FluentValidation.Validators;

namespace AspNetCoreWorkshop.Api.Jobs.UpdateJob
{
    public class UpdateJobRequestValidator : AbstractValidator<UpdateJobRequest>
    {
        public UpdateJobRequestValidator()
        {
            RuleFor(r => r.Data)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Please specify properties to change.")
                .Custom(AllPropsMustHaveMatchingDataTypes);
        }

        private void AllPropsMustHaveMatchingDataTypes(JsonElement data, ValidationContext<UpdateJobRequest> context)
        {
            var properties = typeof(Job).GetProperties();
            
            foreach (var prop in data.EnumerateObject())
            {
                var name = prop.Name;
                var value = prop.Value;

                var property = properties.SingleOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (property == null) {
                    continue;
                }

                try
                {
                    value.Deserialize(property.PropertyType);
                }
                catch (JsonException)
                {
                    context.AddFailure(property.Name, $"Property '{property.Name}' has an invalid type specified.");
                }
            }
        }
    }
}