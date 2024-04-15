using Diploma.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.API;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}