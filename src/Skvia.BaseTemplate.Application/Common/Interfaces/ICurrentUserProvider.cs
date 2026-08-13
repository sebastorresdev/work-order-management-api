using Skvia.BaseTemplate.Application.Features.Auth.DTOs;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

/// <summary>
/// Proveedor para obtener la información del usuario autenticado en la solicitud actual.
/// </summary>
public interface ICurrentUserProvider
{
    /// <summary>
    /// Devuelve los datos principales del usuario actualmente autenticado.
    /// </summary>
    /// <returns>Respuesta con la información del usuario actual.</returns>
    CurrentUserResponse GetCurrentUser();
}

