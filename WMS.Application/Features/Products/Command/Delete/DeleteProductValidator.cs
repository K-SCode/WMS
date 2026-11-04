using FluentValidation;

namespace WMS.Application.Features.Products.Command.Delete
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);
        }
    }
}
