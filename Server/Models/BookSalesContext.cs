﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<Role> Roles { get; set; }
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
            modelBuilder.Entity<Role>()
                .HasIndex(u => u.RoleName)
                .IsUnique()
                .HasDatabaseName("UX_Role_RoleName");

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Staff" },
                new Role { Id = 3, RoleName = "Seller" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, AuthorName = "Dương Ngọc Hải" },
                new Author { Id = 2, AuthorName = "Trần Hiếu Nghĩa" },
                new Author { Id = 3, AuthorName = "Tô Hoài" },
                new Author { Id = 4, AuthorName = "Tố Hữu" },
                new Author { Id = 5, AuthorName = "Nguyễn Trần Phương Tứng" },
                new Author { Id = 6, AuthorName = "Hồ Quỳnh Hương" },
                new Author { Id = 7, AuthorName = "Tun Phạm" },
                new Author { Id = 8, AuthorName = "Tuấn Trần" }
            );

            modelBuilder.Entity<BookSale>().HasData(
                new BookSale { Id = 1, Title = "Đường Hầm Mùa Hạ Tập 1", Quantity = 1000, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 2, Title = "Đường Hầm Mùa Hạ Tập 2", Quantity = 1500, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 3, Title = "Đường Hầm Mùa Hạ Tập 3", Quantity = 500, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 4, Title = "Đường Hầm Mùa Hạ Tập 4", Quantity = 2000, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 5, Title = "Đường Hầm Mùa Hạ Tập 5", Quantity = 1000, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 6, Title = "Đường Hầm Mùa Hạ Tập 6", Quantity = 1500, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 7, Title = "Đường Hầm Mùa Hạ Tập 7", Quantity = 500, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 8, Title = "Đường Hầm Mùa Hạ Tập 8", Quantity = 2000, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 9, Title = "Đường Hầm Mùa Hạ Tập 9", Quantity = 1000, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 10, Title = "Đường Hầm Mùa Hạ Tập 10", Quantity = 1500, Price = 120000, AuthorId = 1 },
                new BookSale { Id = 11, Title = "Dế Mèo Phiêu Lưu Ký Tập 1", Quantity = 500, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 12, Title = "Dế Mèo Phiêu Lưu Ký Tập 2", Quantity = 1000, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 13, Title = "Dế Mèo Phiêu Lưu Ký Tập 3", Quantity = 1500, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 14, Title = "Dế Mèo Phiêu Lưu Ký Tập 4", Quantity = 2000, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 15, Title = "Dế Mèo Phiêu Lưu Ký Tập 5", Quantity = 500, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 16, Title = "Dế Mèo Phiêu Lưu Ký Tập 6", Quantity = 1000, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 17, Title = "Dế Mèo Phiêu Lưu Ký Tập 7", Quantity = 1500, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 18, Title = "Dế Mèo Phiêu Lưu Ký Tập 8", Quantity = 2000, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 19, Title = "Dế Mèo Phiêu Lưu Ký Tập 9", Quantity = 500, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 20, Title = "Dế Mèo Phiêu Lưu Ký Tập 10", Quantity = 1000, Price = 130000, AuthorId = 2 },
                new BookSale { Id = 21, Title = "Hành Trình Đến Bắc Cực Tập 1", Quantity = 1500, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 22, Title = "Hành Trình Đến Bắc Cực Tập 2", Quantity = 2000, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 23, Title = "Hành Trình Đến Bắc Cực Tập 3", Quantity = 500, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 24, Title = "Hành Trình Đến Bắc Cực Tập 4", Quantity = 1000, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 25, Title = "Hành Trình Đến Bắc Cực Tập 5", Quantity = 1500, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 26, Title = "Hành Trình Đến Bắc Cực Tập 6", Quantity = 2000, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 27, Title = "Hành Trình Đến Bắc Cực Tập 7", Quantity = 500, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 28, Title = "Hành Trình Đến Bắc Cực Tập 8", Quantity = 1000, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 29, Title = "Hành Trình Đến Bắc Cực Tập 9", Quantity = 1500, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 30, Title = "Hành Trình Đến Bắc Cực Tập 10", Quantity = 2000, Price = 140000, AuthorId = 3 },
                new BookSale { Id = 31, Title = "Thế Giới Huyền Bí Tập 1", Quantity = 500, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 32, Title = "Thế Giới Huyền Bí Tập 2", Quantity = 1000, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 33, Title = "Thế Giới Huyền Bí Tập 3", Quantity = 1500, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 34, Title = "Thế Giới Huyền Bí Tập 4", Quantity = 2000, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 35, Title = "Thế Giới Huyền Bí Tập 5", Quantity = 500, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 36, Title = "Thế Giới Huyền Bí Tập 6", Quantity = 1000, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 37, Title = "Thế Giới Huyền Bí Tập 7", Quantity = 1500, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 38, Title = "Thế Giới Huyền Bí Tập 8", Quantity = 2000, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 39, Title = "Thế Giới Huyền Bí Tập 9", Quantity = 500, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 40, Title = "Thế Giới Huyền Bí Tập 10", Quantity = 1000, Price = 150000, AuthorId = 4 },
                new BookSale { Id = 41, Title = "Tên Của Bạn Là Tập 1", Quantity = 500, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 42, Title = "Tên Của Bạn Là Tập 2", Quantity = 1000, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 43, Title = "Tên Của Bạn Là Tập 3", Quantity = 1500, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 44, Title = "Tên Của Bạn Là Tập 4", Quantity = 2000, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 45, Title = "Tên Của Bạn Là Tập 5", Quantity = 500, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 46, Title = "Tên Của Bạn Là Tập 6", Quantity = 1000, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 47, Title = "Tên Của Bạn Là Tập 7", Quantity = 1500, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 48, Title = "Tên Của Bạn Là Tập 8", Quantity = 2000, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 49, Title = "Tên Của Bạn Là Tập 9", Quantity = 500, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 50, Title = "Tên Của Bạn Là Tập 10", Quantity = 1000, Price = 150000, AuthorId = 5 },
                new BookSale { Id = 51, Title = "Bên Kia Sông", Quantity = 1500, Price = 160000, AuthorId = 6 },
                new BookSale { Id = 52, Title = "Lạc Trôi", Quantity = 500, Price = 170000, AuthorId = 7 }
            );
        }
    }
}
