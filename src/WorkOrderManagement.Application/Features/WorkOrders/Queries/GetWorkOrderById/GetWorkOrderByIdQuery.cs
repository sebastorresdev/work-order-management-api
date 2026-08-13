using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrderById;

public record GetWorkOrderByIdQuery(Guid Id) : IQuery<ErrorOr<WorkOrderDetailResponse>>;
