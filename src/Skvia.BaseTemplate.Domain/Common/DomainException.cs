namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Excepción personalizada para representar violaciones de reglas de negocio en la capa de dominio.
/// </summary>
/// <param name="message">Mensaje explicativo del error o violación del dominio.</param>
public class DomainException(string message) : Exception(message);

