using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Models
{
    public class BookSalesContext : IdentityDbContext<IdentityUser>
    {
        public BookSalesContext(DbContextOptions<BookSalesContext> options)
           : base(options)
        {
        }
        public DbSet<BookSale> BookSales { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookSale>()
                .HasIndex(bs => bs.Title)
                .IsUnique()
                .HasDatabaseName("UX_BookSale_Title");

            modelBuilder.Entity<Author>()
                .HasIndex(a => a.AuthorName)
                .IsUnique()
                .HasDatabaseName("UX_Author_AuthorName");
        }
    }
}
