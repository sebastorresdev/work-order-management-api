using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Infrastructure.Data.Configurations;

internal class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("work_orders", "business");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TicketNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.TicketNumber)
            .IsUnique();

        builder.Property(x => x.ClientCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ClientName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ClientPhone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ClientSecondaryPhone)
            .HasMaxLength(20);

        builder.Property(x => x.District)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Address)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.AddressReference)
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.ScheduledSlot)
            .HasMaxLength(50);

        builder.Property(x => x.CompletionNotes)
            .HasMaxLength(1000);

        builder.Property(x => x.ObservationNotes)
            .HasMaxLength(1000);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1000);

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(1000);

        // Foreign keys and relationships
        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedTechnician)
            .WithMany()
            .HasForeignKey(x => x.AssignedTechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.StatusHistory)
            .WithOne(x => x.WorkOrder)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ScheduleHistory)
            .WithOne(x => x.WorkOrder)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.StatusHistory).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(x => x.ScheduleHistory).UsePropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for fast querying
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.CreatedByUserId);
        builder.HasIndex(x => x.AssignedTechnicianId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.RequestType);
        builder.HasIndex(x => x.Created);

        // Composite indexes for the most common access patterns in the dashboard and list endpoints.
        builder.HasIndex(x => new { x.BranchId, x.Status, x.Created });
        builder.HasIndex(x => new { x.CreatedByUserId, x.Status, x.Created });
        builder.HasIndex(x => new { x.Status, x.RequestType, x.Created });
    }
}

internal class WorkOrderStatusHistoryConfiguration : IEntityTypeConfiguration<WorkOrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<WorkOrderStatusHistory> builder)
    {
        builder.ToTable("work_order_status_history", "business");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Comments)
            .HasMaxLength(1000);

        builder.HasOne(x => x.ChangedByUser)
            .WithMany()
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.WorkOrderId);
    }
}

internal class WorkOrderScheduleHistoryConfiguration : IEntityTypeConfiguration<WorkOrderScheduleHistory>
{
    public void Configure(EntityTypeBuilder<WorkOrderScheduleHistory> builder)
    {
        builder.ToTable("work_order_schedule_history", "business");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ScheduledSlot)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne(x => x.AssignedTechnician)
            .WithMany()
            .HasForeignKey(x => x.AssignedTechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ScheduledByUser)
            .WithMany()
            .HasForeignKey(x => x.ScheduledByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.WorkOrderId);
    }
}
