using FluentValidation;

namespace WMS.Application.Features.WarehouseTasks.Command.Start
{
    public class StartTaskCommandValidator : AbstractValidator<StartTaskCommand>
    {
        public StartTaskCommandValidator()
        {
            RuleFor(s => s.Id)
                .GreaterThan(0);
        }
    }
}
