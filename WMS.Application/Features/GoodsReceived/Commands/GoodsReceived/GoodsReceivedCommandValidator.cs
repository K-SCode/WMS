using FluentValidation;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public class GoodsReceivedCommandValidator : AbstractValidator<GoodsReceivedCommand>
    {
        public GoodsReceivedCommandValidator()
        {
            RuleFor(property => property.Products)
                .NotEmpty()
                .NotNull();
            RuleForEach(property => property.Products)
                .SetValidator(new GoodsReceviedValidator());
        }
    }
}
