namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Interfaz para la obtención de información sobre zonas horarias del sistema.
/// </summary>
public interface ITimeZoneProvider
{
    /// <summary>
    /// Obtiene la información de la zona horaria a partir de su identificador.
    /// </summary>
    /// <param name="timeZoneId">Identificador de la zona horaria (por ejemplo, 'America/Bogota').</param>
    /// <returns>Instancia de <see cref="TimeZoneInfo"/> correspondiente al identificador provisto.</returns>
    TimeZoneInfo GetTimeZone(string timeZoneId);
}

