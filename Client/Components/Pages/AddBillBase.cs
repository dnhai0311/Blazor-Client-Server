using Microsoft.AspNetCore.Components;
using Blazor_BookSale_Manager.Repositories;
using Blazor_BookSale_Manager.Models;
using Unidecode.NET;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor_BookSale_Manager.Components.Pages
{
    public class AddBillBase : ComponentBase
    {
        [Inject]
        public required IBookSaleRepository bookSaleRepository { get; set; }
        [Inject]
        public required NavigationManager navigationManager { get; set; }

        public Bill bill { get; set; } = new Bill();
        public List<BookSale> bookSales { get; set; } = new List<BookSale>();
        public int selectedBookSaleId = 0;
        public int quantity;
        public string searchText = string.Empty;
        public string ValidationMessage { get; set; } = string.Empty;
        public bool isDiscountApplied { get; set; } = false;
        public double totalPriceWithDiscount => isDiscountApplied ? bill.TotalPrice * 0.9 : bill.TotalPrice;

        public List<BookSale> filteredBookSales =>
            bookSales.Where(bs => string.IsNullOrEmpty(searchText) ||
            bs.Title.Unidecode().Contains(searchText.Unidecode(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        protected override async Task OnInitializedAsync()
        {
            bookSales = await bookSaleRepository.GetAllBookSales();
        }

        public async Task AddBookToBill()
        {
            if (quantity <= 0)
            {
                ValidationMessage = "Số lượng phải lớn hơn 0";
                return;
            }

            var selectedBookSale = bookSales.FirstOrDefault(bs => bs.Id == selectedBookSaleId);
            if (selectedBookSale == null) return;

            var existingBillDetail = bill.BillDetails.FirstOrDefault(bd => bd.BookSale.Id == selectedBookSale.Id);

            if (existingBillDetail != null)
            {
                if (existingBillDetail.Quantity + quantity > selectedBookSale.Quantity)
                {
                    ValidationMessage = $"Số lượng không đủ cho sách: {selectedBookSale.Title}. " +
                        $"Có sẵn: {selectedBookSale.Quantity}, Yêu cầu: {existingBillDetail.Quantity + quantity}.";
                    return;
                }

                existingBillDetail.Quantity += quantity;
                existingBillDetail.Price = existingBillDetail.Quantity * selectedBookSale.Price;

                bill.TotalPrice += selectedBookSale.Price * quantity;

                ValidationMessage = string.Empty;
                quantity = 0;
                selectedBookSaleId = 0;

                return;
            }
            else
            {
                if (quantity > selectedBookSale.Quantity)
                {
                    ValidationMessage = $"Số lượng không đủ cho sách: {selectedBookSale.Title}. " +
                        $"Có sẵn: {selectedBookSale.Quantity}, Yêu cầu: {quantity}.";
                    return;
                }

                var billDetail = new BillDetail
                {
                    BookSaleId = selectedBookSale.Id,
                    BookSale = selectedBookSale,
                    Quantity = quantity,
                    Price = selectedBookSale.Price * quantity
                };

                await Task.Run(() =>
                {
                    bill.BillDetails.Add(billDetail);
                    bill.TotalPrice += billDetail.Price;
                });
            }

            ValidationMessage = string.Empty;
            quantity = 0;
            selectedBookSaleId = 0;
        }

        public void RemoveBookFromBill(BillDetail detail)
        {
            bill.BillDetails.Remove(detail);
            bill.TotalPrice -= detail.Price;
        }

        public async Task HandleValidSubmit()
        {
            if (!bill.BillDetails.Any())
            {
                ValidationMessage = "Vui lòng thêm ít nhất một sách vào hóa đơn.";
                return;
            }
            foreach (var detail in bill.BillDetails)
            {
                detail.BookSale = null;
            }
            bill.TotalPrice = totalPriceWithDiscount;
            await bookSaleRepository.AddBill(bill);
            navigationManager.NavigateTo("/bills/all");
        }
    }
}
