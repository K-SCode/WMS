using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Command.Update
{
    public class UpdateLocationCommandValidation : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidation()
        {
            RuleFor(location => location.Id)
                .GreaterThan(0)
                .LessThan(int.MaxValue);

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
