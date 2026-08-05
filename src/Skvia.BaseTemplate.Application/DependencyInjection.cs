using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scrutor;
using Skvia.BaseTemplate.Application.Common.Behaviors;
using Skvia.BaseTemplate.Application.Common.Messaging;

namespace Skvia.BaseTemplate.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        builder.Services.AddValidatorsFromAssembly(assembly);

        builder.Services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(AuthorizationDecorator.CommandHandler<,>));
        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        builder.Services.TryDecorate(typeof(IQueryHandler<,>), typeof(AuthorizationDecorator.QueryHandler<,>));
        builder.Services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));

        return builder;
    }
}
