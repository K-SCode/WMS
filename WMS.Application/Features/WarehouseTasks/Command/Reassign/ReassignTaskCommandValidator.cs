using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.WarehouseTasks.Command.Reassign
{
    public class ReassignTaskCommandValidator : AbstractValidator<ReassignTaskCommand>
    {
        public ReassignTaskCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.UserId).GreaterThan(0);
        }
    }
}
