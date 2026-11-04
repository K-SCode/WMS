using FluentValidation;

namespace WMS.Application.Features.Products.Query.Get
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);
        }
    }
}
