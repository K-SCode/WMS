using FluentValidation;
using WMS.Application.Features.GoodsReceived.Dto;

namespace WMS.Application.Features.GoodsReceived.Commands
{
    public class DocumentInformationValidator : AbstractValidator<DocumentInformation>
    {
        public DocumentInformationValidator()
        {
            RuleFor(property => property.Comment)
                .MaximumLength(400);

            RuleFor(property => property.InvoiceNumber)
                .MaximumLength(16);

            RuleFor(property => property.SupplierData)
                .SetValidator(new SupplierValidator());
        }
    }
}
