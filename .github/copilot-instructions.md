# Copilot Instructions

## Project Guidelines
- En el proyecto work-order-management, evitar inyectar IApplicationDbContext por constructor en servicios que también son dependencias de los SaveChangesInterceptor de ApplicationDbContext (como ICurrentUserProvider), ya que crea un ciclo de resolución en el DI que cuelga el arranque silenciosamente. Resolver el DbContext de forma perezosa (vía IServiceProvider) en esos casos.
