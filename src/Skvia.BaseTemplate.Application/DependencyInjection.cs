using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scrutor;
using Skvia.BaseTemplate.Application.Common.Behaviors;
using Skvia.BaseTemplate.Application.Common.Messaging;

namespace Skvia.BaseTemplate.Application;

/// <summary>
/// Proporciona los métodos de extensión para el registro de servicios de la capa de Aplicación en el contenedor de inyección de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos los validadores, comandos, consultas y decoradores (Logging, Validación, Autorización) de la capa de Aplicación.
    /// </summary>
    /// <param name="builder">Constructor de la aplicación host.</param>
    /// <returns>El mismo <see cref="IHostApplicationBuilder"/> con los servicios registrados.</returns>
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Obtención del ensamblado actual de la capa de aplicación
        var assembly = typeof(DependencyInjection).Assembly;

        // Registra automáticamente todos los validadores de FluentValidation encontrados en el ensamblado
        builder.Services.AddValidatorsFromAssembly(assembly);

        // Escaneo y registro automático de CommandHandlers y QueryHandlers con ciclo de vida Scoped
        builder.Services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        // Decoradores para CommandHandlers: Autorización -> Validación -> Logging
        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(AuthorizationDecorator.CommandHandler<,>));
        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        builder.Services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        // Decoradores para QueryHandlers: Autorización -> Logging
        builder.Services.TryDecorate(typeof(IQueryHandler<,>), typeof(AuthorizationDecorator.QueryHandler<,>));
        builder.Services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));

        return builder;
    }
}
