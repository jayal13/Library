using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class DataContext : DbContext
{
    private readonly IConfiguration _config;

    public DataContext(IConfiguration config)
    {
        _config = config;
    }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
            optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        }
    }
}