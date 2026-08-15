using FluentAssertions;
using Moq;
using WorkOrderManagement.Application.Features.Branches.Commands.UpdateBranch;
using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Tests;

public class BranchUseCaseTests
{
    [Fact]
    public async Task UpdateBranch_WhenCodeAlreadyExists_ReturnsDuplicateBranchErrorForCode()
    {
        var branchId = Guid.NewGuid();
        var existingBranch = Branch.Create("OLD", "Sucursal vieja", "Calle 1").Value;
        var repository = new Mock<IBranchRepository>();

        repository
            .Setup(x => x.GetEntityByIdAsync(branchId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBranch);

        repository
            .Setup(x => x.ExistsByCodeAsync("NEW", branchId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new UpdateBranchCommandHandler(repository.Object);
        var command = new UpdateBranchCommand(branchId, "NEW", "Sucursal nueva", "Calle 2");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Branch.DuplicateBranch");
        result.FirstError.Description.Should().Be(BranchErrors.DuplicateBranch("NEW").Description);
    }
}
