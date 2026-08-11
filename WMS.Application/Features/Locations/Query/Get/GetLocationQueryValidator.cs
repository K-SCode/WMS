using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Locations.Query.Get
{
    public class GetLocationQueryValidator : AbstractValidator<GetLocationQuery>
    {
        public GetLocationQueryValidator() 
        {
            RuleFor(location => location.Id)
                .GreaterThan(0);
        }
    }
}
