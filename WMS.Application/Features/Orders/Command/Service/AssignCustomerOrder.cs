using Mapster;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class AssignCustomerOrder(IUnitOfWork unitOfWorks) : IAssignCustomerOrder
    {
        public async Task<Customer> AssignAsync(Customer request,CancellationToken ct)
        {
            Customer? customer = await unitOfWorks.Customer.GetByEmailAsync(request.Email, ct);
            if (customer is null)
            {
                customer = await Add(request, ct);
            }
            return customer;
        }

        private async Task<Customer> Add(Customer request, CancellationToken ct)
        {
            Customer customer = request.Adapt<Customer>();
            await unitOfWorks.Customer.AddAsync(customer, ct);
            return customer;
        }
    }
}

