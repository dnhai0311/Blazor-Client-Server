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

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                author = await AuthorRepository.GetAuthorById(Id.Value);
            }
        }

        public async Task HandleValidSubmit()
        {
            if (author.Id == 0)
            {
                await AuthorRepository.AddAuthor(author); 
            }
            else
            {
                await AuthorRepository.UpdateAuthor(author); 
            }
            NavigationManager.NavigateTo("/authors/all"); 
        }
    }
}
