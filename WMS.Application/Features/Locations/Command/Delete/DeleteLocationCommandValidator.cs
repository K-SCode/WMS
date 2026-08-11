using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Command.Delete
{
    public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
    {
        public DeleteLocationCommandValidator()
        {
            RuleFor(location => location.Id)
                .GreaterThan(0);
        }
    }
}
