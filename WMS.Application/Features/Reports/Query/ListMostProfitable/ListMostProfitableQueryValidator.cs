using FluentValidation;
using WMS.Application.Features.Raports.Query.GetMostProfitable;

namespace WMS.Application.Features.Reports.Query.ListMostProfitable
{
    public class ListMostProfitableQueryValidator : AbstractValidator<ListMostProfitableQuery>
    {
        private const int MaxDays = 730;
        public ListMostProfitableQueryValidator()
        {
            RuleFor(p => p.ToTime)
                .GreaterThan(p => p.FromTime);
            RuleFor(p => p.Peroid)
                .IsInEnum();
            RuleFor(p => p)
                .Must(q => (q.ToTime - q.FromTime).TotalDays <= MaxDays);
        }
    }
}
