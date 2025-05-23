using Microsoft.EntityFrameworkCore;
using BehaviorDetection.Domain.Entities;

namespace BehaviorDetection.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<BehaviorEvent> BehaviorEvents => Set<BehaviorEvent>();
}
