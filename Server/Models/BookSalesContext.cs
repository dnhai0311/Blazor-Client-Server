using Microsoft.EntityFrameworkCore;

namespace Blazor_BookSale_Manager.Models
{
    public class BookSalesContext : DbContext
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
            modelBuilder.Entity<BookSale>()
                 .HasIndex(bs => bs.Title)
                 .IsUnique()
                 .HasDatabaseName("UX_BookSale_Title");
        }

    }
}
