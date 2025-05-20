using Microsoft.EntityFrameworkCore;
using ProductNotifier.Models;

namespace ProductNotifier;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options)
    {

    }

    public DbSet<Record> Record { get; set; }
}
