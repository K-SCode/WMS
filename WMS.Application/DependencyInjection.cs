using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WMS.Application.Behaviors;
using WMS.Application.Features.Locations.Command.Create;
using WMS.Application.Features.Products.Command.Create;

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
            return services;
        }
    }
}
