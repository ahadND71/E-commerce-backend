using Microsoft.EntityFrameworkCore;

// todo: work in progress
public class db_context : DbContext
{
    public db_context(DbContextOptions<db_context> options) : base(options) { }

    public DbSet<customer> customers { get; set; }
    public DbSet<admin> admins { get; set; }
    public DbSet<address> addresses { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("ConnectionString");
    }
}
