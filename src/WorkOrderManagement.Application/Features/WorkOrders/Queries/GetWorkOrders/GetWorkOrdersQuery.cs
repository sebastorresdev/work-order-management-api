using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public record GetWorkOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    WorkOrderStatus? Status = null,
    WorkOrderType? RequestType = null,
    Guid? BranchId = null,
    Guid? CurrentUserId = null,
    List<string>? UserRoles = null) : IQuery<ErrorOr<PaginatedResponse<WorkOrderResponse>>>;
