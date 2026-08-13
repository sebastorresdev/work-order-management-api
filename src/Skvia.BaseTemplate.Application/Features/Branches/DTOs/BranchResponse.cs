namespace Skvia.BaseTemplate.Application.Features.Branches.DTOs;

/// <summary>
/// DTO de respuesta con la información básica de una sede.
/// </summary>
/// <param name="Id">Identificador único de la sede.</param>
/// <param name="Code">Código identificador de la sede.</param>
/// <param name="Name">Nombre descriptivo de la sede.</param>
/// <param name="Address">Dirección física opcional de la sede.</param>
public record BranchResponse(
    Guid Id,
    string Code,
    string Name,
    string? Address);

