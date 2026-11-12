using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    // public DbSet<> 
}