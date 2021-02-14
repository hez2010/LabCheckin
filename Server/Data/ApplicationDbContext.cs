using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LabCheckin.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<CheckinRecord> Records { get; set; } = default!;
        public virtual DbSet<MonthlySalary> Salaries { get; set; } = default!;

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
            });
            builder.Entity<MonthlySalary>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.UserId);
            });
        }
    }
}
