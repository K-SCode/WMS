using FluentValidation;

namespace WMS.Application.Features.Auth.Query.Get.GetUserDetails
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(u => u.Id)
                .GreaterThan(0);
        }
    }
}
