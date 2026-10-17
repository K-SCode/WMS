using Mapster;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class AssignOrderUser(IUnitOfWorks unitOfWorks) : IAssignOrderUser
    {
        public async Task<Customer> AssignAsync(Customer request,CancellationToken ct)
        {
            Customer? customer = await unitOfWorks.Customer.GetByEmailAsync(request.Email, ct);
            if (customer is null)
            {
                customer = request.Adapt<Customer>();
                await unitOfWorks.Customer.AddAsync(customer, ct);
            }
            return customer;
        }
    }
}

