using FluentValidation;
using WMS.Application.Features.GoodsReceived.Dto;

namespace WMS.Application.Features.GoodsReceived.Commands
{
    public class SupplierValidator : AbstractValidator<SupplierInformation>
    {
        public SupplierValidator()
        {
            RuleFor(property => property.Address)
                .NotEmpty().NotNull();
            RuleFor(property => property.CompanyName)
                .NotEmpty().NotNull();
            RuleFor(property => property.Nip)
                .Length(10);
            RuleFor(property => property.Phone)
                .MaximumLength(11);
        }
    }
}
