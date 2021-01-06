using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSharing.Data
{
    public class BookSharingDbContext : IdentityDbContext
    {
        public BookSharingDbContext(DbContextOptions<BookSharingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}