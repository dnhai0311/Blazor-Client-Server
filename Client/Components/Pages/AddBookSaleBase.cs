using Microsoft.AspNetCore.Components;
using Client.Repositories;
using Server.Models;

namespace Client.Components.Pages
{
    public class AddBookSaleBase : ComponentBase
    {
        [Inject]
        public required IBookSaleRepository bookSaleRepository { get; set; }

        [Inject]
        public required NavigationManager navigationManager { get; set; }

        [Parameter]
        public int? Id { get; set; }

        public BookSale bookSale { get; set; } = new BookSale();
        public List<Author> authors { get; set; } = new List<Author>();

        protected override async Task OnInitializedAsync()
        {
            authors = await bookSaleRepository.GetAllAuthors(); 

            if (Id.HasValue)
            {
                bookSale = await bookSaleRepository.GetBookSaleById(Id.Value);
            }
        }

        public async Task HandleValidSubmit()
        {
            if (bookSale.Id == 0)
            {
                await bookSaleRepository.AddBookSale(bookSale);
            }
            else
            {
                await bookSaleRepository.UpdateBookSale(bookSale);
            }
            navigationManager.NavigateTo("/booksales/all");
        }
    }
}
