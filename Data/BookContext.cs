using BookSharing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSharing.Data
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Book>();
            builder.Entity<Orders>().HasOne<Book>("Book").WithMany("Orders").HasForeignKey("BookId");
        }
    }
}