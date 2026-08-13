namespace Skvia.BaseTemplate.Domain.Employees;

/// <summary>
/// Contiene las constantes con los límites de longitud para las propiedades de los empleados.
/// </summary>
public static class EmployeeConstants
{
    /// <summary>
    /// Longitud máxima permitida para el código de empleado.
    /// </summary>
    public const int CodeMaxLength = 20;

    /// <summary>
    /// Longitud máxima permitida para los nombres del empleado.
    /// </summary>
    public const int FirstNameMaxLength = 100;

    /// <summary>
    /// Longitud máxima permitida para los apellidos del empleado.
    /// </summary>
    public const int LastNameMaxLength = 100;

    /// <summary>
    /// Longitud máxima permitida para el número de documento de identidad.
    /// </summary>
    public const int DocumentNumberMaxLength = 30;

    /// <summary>
    /// Longitud máxima permitida para el correo electrónico.
    /// </summary>
    public const int EmailMaxLength = 150;

    /// <summary>
    /// Longitud máxima permitida para el número telefónico.
    /// </summary>
    public const int PhoneMaxLength = 20;

    /// <summary>
    /// Longitud máxima permitida para el cargo o posición laboral.
    /// </summary>
    public const int PositionMaxLength = 100;

    /// <summary>
    /// Longitud máxima permitida para el departamento u área de trabajo.
    /// </summary>
    public const int DepartmentMaxLength = 100;

    /// <summary>
    /// Longitud máxima permitida para la URL de la foto de perfil.
    /// </summary>
    public const int PhotoUrlMaxLength = 500;
}

