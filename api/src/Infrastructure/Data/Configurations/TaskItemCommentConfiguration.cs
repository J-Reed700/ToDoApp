using ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoApp.Infrastructure.Data.Configurations;

public class TaskItemCommentConfiguration : IEntityTypeConfiguration<TaskItemComment>
{
    public void Configure(EntityTypeBuilder<TaskItemComment> builder)
    {
        builder.Property(c => c.Comment)
            .HasMaxLength(1000);

        builder.HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
