using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.CreateOrder
{
    public class CreateOrderHandler(
        ICreateOrderProcess process
        ) : ICommandHandler<CreateOrderCommand, Result<int>>
    {
        public async ValueTask<Result<int>> Handle(
            CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            if(VerifyNip(command.Customer.Nip))
            {
                return Errors<Customer>.InvalidProperty(nameof(command.Customer.Nip));
            }

            int id = await CreateOrder(
                command.Customer,
                command.Products,
                command.Information,
                cancellationToken);

            return id;
        }
        private static bool VerifyNip(string? nip) =>
            NipIsNotNull(nip) && !NipIsCorrect(nip!);
        private static bool NipIsNotNull(string? nip) => nip is not null;
        private static bool NipIsCorrect(string nip)
        {
            if (LengthIsCorrect(nip))
                return false;

            int number = CalcControlNumber(nip);

            if (!ControlNumberIsValid(number, nip))
            {
                return false;
            }
            return true;
        }
        private static bool LengthIsCorrect(string nip) => nip.Length != 10;
        private static int CalcControlNumber(string nip)
        {
            int[] nipWeights = [6, 5, 7, 2, 3, 4, 5, 6, 7];
            int sum = 0;
            for (int i = 0; i < nip.Length - 1; i++)
            {
                sum += int.Parse(nip[i].ToString()) * nipWeights[i];
            }

            int controlNumber = (int)(sum % 11);
            return controlNumber;
        }
        private static bool ControlNumberIsValid(int number, string nip) =>
            number == 10 || number == int.Parse(nip[^1].ToString());

        private async Task<int> CreateOrder(
            CustomerInformation customer,
            IReadOnlyCollection<CreateProductOrder> products,
            OrderInformation information,
            CancellationToken ct)
        {
            return await process.ProcessAsync(
                customer,
                products,
                information,
                ct);
        }
    }
}
