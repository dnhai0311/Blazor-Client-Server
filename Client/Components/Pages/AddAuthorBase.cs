using Microsoft.AspNetCore.Components;
using Blazor_BookSale_Manager.Repositories;
using Blazor_BookSale_Manager.Models;

namespace Blazor_BookSale_Manager.Components.Pages
{
    public class AddAuthorBase : ComponentBase
    {
        [Inject]
        public required IBookSaleRepository bookSaleRepository { get; set; } 
        [Inject]
        public required NavigationManager navigationManager { get; set; }
        [Parameter]
        public int? Id { get; set; }

        public Author author { get; set; } = new Author();

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
            {
                author = await bookSaleRepository.GetAuthorById(Id.Value);
            }
        }

        public async Task HandleValidSubmit()
        {
            if (author.Id == 0)
            {
                await bookSaleRepository.AddAuthor(author); 
            }
            else
            {
                await bookSaleRepository.UpdateAuthor(author); 
            }
            navigationManager.NavigateTo("/authors/all"); 
        }
    }
}
