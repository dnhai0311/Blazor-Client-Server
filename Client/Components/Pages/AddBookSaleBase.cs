using Microsoft.AspNetCore.Components;
using Shared.Repositories;
using Shared.Models;
using Client.Services;

namespace Client.Components.Pages
{
    public class AddBookSaleBase : ComponentBase
    {
        [Inject]
        public required IAuthorRepository AuthorRepository { get; set; }
        [Inject]
        public required IBookSaleRepository BookSaleRepository { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required NotificationService NotificationService { get; set; }

        [Parameter]
        public int? Id { get; set; }

        public BookSale bookSale { get; set; } = new BookSale();
        public List<Author> authors { get; set; } = new List<Author>();

        public string searchText = string.Empty;

        public string errorMessage { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            authors = await AuthorRepository.GetAllAuthors();

            if (Id.HasValue)
            {
                bookSale = await BookSaleRepository.GetBookSaleById(Id.Value);
            }
        }

        public async Task HandleValidSubmit()
        {
            errorMessage = string.Empty;

            try
            {
                if (bookSale.Id == 0)
                {
                    await BookSaleRepository.AddBookSale(bookSale);
                    NotificationService.ShowSuccessMessage("Thêm mới tác phẩm thành công!");

                }
                else
                {
                    await BookSaleRepository.UpdateBookSale(bookSale);
                    NotificationService.ShowSuccessMessage("Sửa tác phẩm thành công!");
                }
                NavigationManager.NavigateTo("/booksales");
            }
            catch (ApplicationException ex)
            {
                errorMessage = ex.Message;
                NotificationService.ShowErrorMessage(errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = $"Có lỗi xảy ra: {ex.Message}";
                NotificationService.ShowErrorMessage(errorMessage);
            }
        }
    }
}
