using FluentValidation;

namespace WMS.Application.Features.GoodsReceived.Query.Get
{
    public class GetGoodsReceivedQueryValidator : AbstractValidator<GetGoodsReceivedQuery>
    {
        public GetGoodsReceivedQueryValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0);
        }
    }
}
