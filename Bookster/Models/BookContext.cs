using Microsoft.EntityFrameworkCore;

namespace Booker.Models;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public BookContext(DbContextOptions options) : base(options)
    {
    }
}