using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WMS.Application.Behaviors;
using WMS.Application.common.Service;
using WMS.Application.Features.GoodsReceived.Services;
using WMS.Application.Features.Orders.Command.Service;
using WMS.Application.Features.Products.Command.Create;
using WMS.Application.Interfaces;

namespace WMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.Assemblies = [typeof(CreateProductCommand)];
                options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IIncreaseQuantity, IncreaseQunatity>();
            services.AddScoped<IDecreaseQuantity, DecreaseQuantity>();

            services.AddScoped<ISaveGoodsReceipt, SaveGoodsReceipt>();
            services.AddScoped<IGoodsReceivedProcess, GoodsReceivedProcess>();

            services.AddScoped<ICreateOrderProcess, CreateOrderProcess>();
            services.AddScoped<IAssignOrderItems, AssignOrderItems>();
            services.AddScoped<IAssignOrderUser, AssignOrderUser>();
            services.AddScoped<IOrderRegistration, OrderRegistration>();

            services.AddScoped<ICreateWarehouseTask, CreateWarehouseTask>();
            return services;
        }
    }
}
