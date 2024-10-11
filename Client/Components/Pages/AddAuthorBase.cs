using Microsoft.AspNetCore.Components;
using Shared.Repositories;
using Shared.Models;

namespace Client.Components.Pages
{
    public class AddAuthorBase : ComponentBase
    {
        [Inject]
        public required IAuthorRepository AuthorRepository { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Parameter]
        public int? Id { get; set; }

        public Author author { get; set; } = new Author();
        public string errorMessage { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                author = await AuthorRepository.GetAuthorById(Id.Value);
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
                }
                else
                {
                    await AuthorRepository.UpdateAuthor(author);
                }

                NavigationManager.NavigateTo("/authors");
            }
            catch (ApplicationException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = $"Có lỗi xảy ra: {ex.Message}";
            }
        }
    }
}
