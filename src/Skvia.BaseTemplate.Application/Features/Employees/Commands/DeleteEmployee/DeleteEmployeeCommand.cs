namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;

