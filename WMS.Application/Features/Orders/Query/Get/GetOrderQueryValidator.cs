using FluentValidation;

namespace WMS.Application.Features.Orders.Query.Get
{
    public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
    {
        public GetOrderQueryValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);
        }
    }
}
