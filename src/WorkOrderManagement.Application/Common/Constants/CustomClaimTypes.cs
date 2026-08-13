namespace WorkOrderManagement.Application.Common.Constants;

/// <summary>
/// Define constantes para los tipos de claims personalizados utilizados en los tokens JWT del sistema.
/// </summary>
public static class CustomClaimTypes
{
    /// <summary>
    /// Clave del claim de permisos individuales inyectados en la identidad del usuario.
    /// </summary>
    public const string Permission = "permissions";
}

