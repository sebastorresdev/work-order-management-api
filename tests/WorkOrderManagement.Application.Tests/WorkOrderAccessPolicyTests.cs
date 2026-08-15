using FluentAssertions;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Tests;

public class WorkOrderAccessPolicyTests
{
    [Fact]
    public void ResolveScope_WhenUserIsAdminAndBranchFilterIsPresent_ReturnsBranchScope()
    {
        var query = new GetWorkOrdersQuery(
            CurrentUserId: Guid.NewGuid(),
            UserRoles: ["Admin"],
            BranchId: Guid.NewGuid());

        var result = WorkOrderAccessPolicy.ResolveScope(query, [], []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByBranch);
        result.BranchIds.Should().ContainSingle().Which.Should().Be(query.BranchId!.Value);
    }

    [Fact]
    public void ResolveScope_WhenUserIsSupervisor_ReturnsTeamScope()
    {
        var currentUserId = Guid.NewGuid();
        var subordinateId = Guid.NewGuid();

        var query = new GetWorkOrdersQuery(
            CurrentUserId: currentUserId,
            UserRoles: ["Supervisor"]);

        var result = WorkOrderAccessPolicy.ResolveScope(query, [], [subordinateId]);

        result.Mode.Should().Be(WorkOrderAccessMode.ByTeam);
        result.UserIds.Should().Contain(currentUserId);
        result.UserIds.Should().Contain(subordinateId);
    }

    [Fact]
    public void ResolveScope_WhenUserIsRegularUser_ReturnsUserScope()
    {
        var currentUserId = Guid.NewGuid();

        var query = new GetWorkOrdersQuery(
            CurrentUserId: currentUserId,
            UserRoles: ["Vendedor"]);

        var result = WorkOrderAccessPolicy.ResolveScope(query, [], []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByUser);
        result.UserIds.Should().ContainSingle().Which.Should().Be(currentUserId);
    }

    [Fact]
    public void ResolveScope_WhenUserIsBackofficeAndHasBranchAssigned_ReturnsAssignedBranchScope()
    {
        var currentUserId = Guid.NewGuid();
        var assignedBranchId = Guid.NewGuid();
        var requestedBranchId = Guid.NewGuid();

        var query = new GetWorkOrdersQuery(
            CurrentUserId: currentUserId,
            UserRoles: ["Backoffice"],
            BranchId: requestedBranchId);

        var result = WorkOrderAccessPolicy.ResolveScope(query, [assignedBranchId], []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByBranch);
        result.BranchIds.Should().ContainSingle().Which.Should().Be(assignedBranchId);
        result.BranchIds.Should().NotContain(requestedBranchId);
    }

    [Fact]
    public void ResolveScope_WhenUserHasMultipleBranchesAssigned_ReturnsAllAssignedBranchIds()
    {
        var currentUserId = Guid.NewGuid();
        var branch1 = Guid.NewGuid();
        var branch2 = Guid.NewGuid();
        var branch3 = Guid.NewGuid();

        var query = new GetWorkOrdersQuery(
            CurrentUserId: currentUserId,
            UserRoles: ["Backoffice"]);

        var result = WorkOrderAccessPolicy.ResolveScope(query, [branch1, branch2, branch3], []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByBranch);
        result.BranchIds.Should().HaveCount(3);
        result.BranchIds.Should().Contain([branch1, branch2, branch3]);
    }

    [Fact]
    public void ResolveScope_WhenUserHasNoRoles_ReturnsAllScope()
    {
        var query = new GetWorkOrdersQuery(CurrentUserId: Guid.NewGuid(), UserRoles: []);

        var result = WorkOrderAccessPolicy.ResolveScope(query, [], []);

        result.Mode.Should().Be(WorkOrderAccessMode.All);
    }
}
