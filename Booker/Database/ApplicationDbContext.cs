using Booker.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booker.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Cart>()
            .HasKey(ub => new { ub.UserId, ub.BookId });

        modelBuilder.Entity<Cart>()
            .HasOne(ub => ub.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(ub => ub.UserId);

        modelBuilder.Entity<Cart>()
            .HasOne(ub => ub.Book)
            .WithMany(b => b.Carts)
            .HasForeignKey(ub => ub.BookId);
    }
}