using FluentValidation;
using FluentValidation.Validators;

namespace WMS.Application.Features.WarehouseTasks.Command.Complete
{
    public class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
    {
        public CompleteTaskCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
        }
    }
}
