﻿using Microsoft.EntityFrameworkCore;
using Server.Models;
using Shared.Models;
using Shared.Repositories;

namespace Server.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly BookSalesContext bookSalesContext;

        public BillRepository(BookSalesContext context)
        {
            bookSalesContext = context;
        }

        public async Task<List<Bill>> GetAllBills()
        {
            return await bookSalesContext.Bills
                .OrderBy(bill => bill.Id)
                .Include(user => user.User)
                .ToListAsync();
        }

        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await bookSalesContext.Bills
                    .Include(b => b.BillDetails)
                        .ThenInclude(bd => bd.BookSale)
                    .Include(user => user.User)
                    .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                throw new KeyNotFoundException($"Bill với ID: {id} không tìm thấy.");
            }

            return bill;
        }

        public async Task AddBill(Bill bill)
        {

            var user = await bookSalesContext.Users
                    .FindAsync(bill.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {bill.UserId} không tìm thấy.");
            }

            foreach (var detail in bill.BillDetails)
            {
                var bookSale = await bookSalesContext.BookSales
                    .FindAsync(detail.BookSaleId);

                if (bookSale == null)
                {
                    throw new KeyNotFoundException($"BookSale với ID: {detail.BookSaleId} không tìm thấy.");
                }

                if (bookSale.Quantity < detail.Quantity)
                {
                    throw new InvalidOperationException($"Không đủ số lượng sách cho BookSale với ID: {detail.BookSaleId}. Số lượng còn lại: {bookSale.Quantity}");
                }

                bookSale.Quantity -= detail.Quantity;
            }

            bookSalesContext.Bills.Add(bill);
            await bookSalesContext.SaveChangesAsync();
        }
    }
}
