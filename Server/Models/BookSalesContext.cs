using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Models
{
    public class BookSalesContext : DbContext
    {
        public BookSalesContext(DbContextOptions<BookSalesContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BookSale> BookSales { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(60)
                .IsRequired();
            modelBuilder.Entity<BookSale>()
                .HasIndex(bs => bs.Title)
                .IsUnique()
                .HasDatabaseName("UX_BookSale_Title");

            modelBuilder.Entity<Author>()
                .HasIndex(a => a.AuthorName)
                .IsUnique()
                .HasDatabaseName("UX_Author_AuthorName");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("UX_User_UserName");
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("UX_User_Email");
        }
    }
}
