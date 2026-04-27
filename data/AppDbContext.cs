using Microsoft.EntityFrameworkCore;
using learn_dotnet.models;

namespace learn_dotnet.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<User> Users => Set<User>();
}
