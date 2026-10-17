using FluentValidation;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public class GoodsReceivedCommandValidator : AbstractValidator<GoodsReceivedCommand>
    {
        public GoodsReceivedCommandValidator()
        {
            RuleFor(property => property.GoodsReceivedNote)
                .NotNull();

            RuleForEach(property => property.GoodsReceivedNote.Products)
                .SetValidator(new GoodsReceviedValidator());

            RuleFor(property => property.GoodsReceivedNote.Information)
                .SetValidator(new DocumentInformationValidator());
        }
    }
}
