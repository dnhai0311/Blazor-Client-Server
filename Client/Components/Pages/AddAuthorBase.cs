using Microsoft.AspNetCore.Components;
using Shared.Repositories;
using Shared.Models;
using Client.Services;

namespace Client.Components.Pages
{
    public class AddAuthorBase : ComponentBase
    {
        [Inject]
        public required IAuthorRepository AuthorRepository { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required NotificationService NotificationService { get; set; }
        [Parameter]
        public int? Id { get; set; }
        [Parameter]
        public bool IsDetail { get; set; }

        public Author author { get; set; } = new Author();
        public string errorMessage { get; set; } = string.Empty;



        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                try
                {
                    author = await AuthorRepository.GetAuthorById(Id.Value);
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

            try
            {
                if (author.Id == 0)
                {
                    await AuthorRepository.AddAuthor(author);
                    NotificationService.ShowSuccessMessage("Thêm mới tác giả thành công!");

                }
                else
                {
                    await AuthorRepository.UpdateAuthor(author);
                    NotificationService.ShowSuccessMessage("Sửa đổi tác giả thành công!");

                }

                NavigationManager.NavigateTo("/authors");
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
