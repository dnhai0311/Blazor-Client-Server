using Microsoft.AspNetCore.Components;
using Shared.Repositories;
using Shared.Models;
using Client.Services;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

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
        [Parameter]
        public bool IsView { get; set; }

        public BookSale bookSale { get; set; } = new BookSale();
        public List<Author> authors { get; set; } = new List<Author>();

        public string searchText = string.Empty;

        public string errorMessage { get; set; } = string.Empty;

        public IBrowserFile file;
        public string imagePreview = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            authors = await AuthorRepository.GetAllAuthors();

            if (Id.HasValue)
            {
                try
                {
                    bookSale = await BookSaleRepository.GetBookSaleById(Id.Value);
                }
                catch (ApplicationException ex)
                {
                    NotificationService.ShowErrorMessage(ex.Message);
                    NavigationManager.NavigateTo("/");
                }
            }
        }

        public async Task HandleValidSubmit()
        {
            errorMessage = string.Empty;
            if (file == null && bookSale.Id == 0) return; 

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

        public async Task UploadFiles(IBrowserFile selectedFile)
        {
            if (selectedFile == null) return;
            file = selectedFile;
            imagePreview = string.Empty;

            using (var stream = file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var buffer = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(buffer);

                    imagePreview = $"data:{file.ContentType};base64,{base64String}";
                    bookSale.ImgUrl = imagePreview;
                }
            }
        }


    }
}
