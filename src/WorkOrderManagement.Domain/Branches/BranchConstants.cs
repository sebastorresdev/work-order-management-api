namespace WorkOrderManagement.Domain.Branches;

/// <summary>
/// Contiene las constantes de validación y límites de longitud para las sedes/sucursales.
/// </summary>
public static class BranchConstants
{
    /// <summary>
    /// Longitud máxima permitida para el código identificador de la sede.
    /// </summary>
    public const int CodeMaxLength = 20;

    /// <summary>
    /// Longitud máxima permitida para el nombre comercial o descriptivo de la sede.
    /// </summary>
    public const int NameMaxLength = 150;

    /// <summary>
    /// Longitud máxima permitida para la dirección física de la sede.
    /// </summary>
    public const int AddressMaxLength = 300;
}

