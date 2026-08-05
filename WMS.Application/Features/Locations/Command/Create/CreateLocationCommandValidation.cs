using FluentValidation;

namespace WMS.Application.Features.Locations.Command.Create
{
    public class CreateLocationCommandValidation : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidation()
        {
            RuleFor(location => location.Sector)
                .Must(char.IsLetter)
                .WithMessage("Sector is required");

            RuleFor(location => location.Rack)
                .GreaterThan(0);
            RuleFor(location => location.Number)
                .InclusiveBetween(0, 10);
            RuleFor(location => location.IsFull)
                .Equal(false);
        }
    }
}
