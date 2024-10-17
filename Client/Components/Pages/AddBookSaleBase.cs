using Microsoft.AspNetCore.Components;
using Shared.Repositories;
using Shared.Models;
using Client.Services;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

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

        public List<string> imagePreviews = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            authors = await AuthorRepository.GetAllAuthors();

            if (Id.HasValue)
            {
                try
                {
                    bookSale = await BookSaleRepository.GetBookSaleById(Id.Value);
                    imagePreviews = JsonSerializer.Deserialize<List<string>>(bookSale.ImgUrl)!;
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

            if(imagePreviews.Count == 0 )
            {
                NotificationService.ShowErrorMessage("Vui lòng tải lên ít nhất 1 ảnh");
                return;
            }

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

        public async Task UploadFiles(IReadOnlyList<IBrowserFile> selectedFiles)
        {
            if (selectedFiles.Count == 0) return;

            if (imagePreviews.Count + selectedFiles.Count > 4)
            {
                NotificationService.ShowErrorMessage($"Chỉ nhận tối đã 4 ảnh~~\n Còn có thể tải lên {4-imagePreviews.Count} ảnh~~");
                return;
            }

            foreach (var file in selectedFiles)
            {

                var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);

                using (var stream = resizedFile.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        var buffer = memoryStream.ToArray();
                        var base64String = Convert.ToBase64String(buffer);

                        var imagePreview = $"data:{file.ContentType};base64,{base64String}";
                        imagePreviews.Add(imagePreview);
                    }
                }
            }

            bookSale.ImgUrl = JsonSerializer.Serialize(imagePreviews, new JsonSerializerOptions { WriteIndented = true });
        }

        public void RemoveImage(int index)
        {
            if (index < 0 || index >= imagePreviews.Count)
            {
                return;
            }

            imagePreviews.RemoveAt(index);

            bookSale.ImgUrl = JsonSerializer.Serialize(imagePreviews, new JsonSerializerOptions { WriteIndented = true });

            Console.WriteLine(index);

        }

    }
}
