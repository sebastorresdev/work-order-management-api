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

        var result = WorkOrderAccessPolicy.ResolveScope(query, null, []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByBranch);
        result.BranchId.Should().Be(query.BranchId);
    }

    [Fact]
    public void ResolveScope_WhenUserIsSupervisor_ReturnsTeamScope()
    {
        var currentUserId = Guid.NewGuid();
        var subordinateId = Guid.NewGuid();

        var query = new GetWorkOrdersQuery(
            CurrentUserId: currentUserId,
            UserRoles: ["Supervisor"]);

        var result = WorkOrderAccessPolicy.ResolveScope(query, null, [subordinateId]);

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

        var result = WorkOrderAccessPolicy.ResolveScope(query, null, []);

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

        var result = WorkOrderAccessPolicy.ResolveScope(query, assignedBranchId, []);

        result.Mode.Should().Be(WorkOrderAccessMode.ByBranch);
        result.BranchId.Should().Be(assignedBranchId);
        result.BranchId.Should().NotBe(requestedBranchId);
    }

    [Fact]
    public void ResolveScope_WhenUserHasNoRoles_ReturnsAllScope()
    {
        var query = new GetWorkOrdersQuery(CurrentUserId: Guid.NewGuid(), UserRoles: []);

        var result = WorkOrderAccessPolicy.ResolveScope(query, null, []);

        result.Mode.Should().Be(WorkOrderAccessMode.All);
    }
}
