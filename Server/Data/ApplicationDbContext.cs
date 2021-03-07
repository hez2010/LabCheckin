using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LabCenter.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CheckinRecord> Records { get; set; } = default!;
        public DbSet<MonthlySalary> Salaries { get; set; } = default!;
        public DbSet<WorkPlan> Plans { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CheckinRecord>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.UserId);

                entity.HasOne(d => d.WorkPlan)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.WorkPlanId);
            });

            builder.Entity<MonthlySalary>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.UserId);
            });

            builder.Entity<WorkPlan>(entity =>
            {
                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Plans);

                entity.Property(d => d.StartTime).UsePropertyAccessMode(PropertyAccessMode.Property);
            });
        }
    }
}
