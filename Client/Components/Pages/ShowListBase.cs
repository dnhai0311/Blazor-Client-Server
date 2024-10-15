using Shared.Models;
using Shared.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Unidecode.NET;
using MudBlazor;

namespace Client.Components.Pages
{
    public class ShowListBase : ComponentBase
    {
        [Parameter]
        public string? Type { get; set; }
        [Parameter]
        public int? AuthorId { get; set; }
        [Inject]
        public required IBookSaleRepository BookSaleRepository { get; set; }
        [Inject]
        public required IAuthorRepository AuthorRepository { get; set; }
        [Inject]
        public required IBillRepository BillRepository { get; set; }
        [Inject]
        public required IUserClientRepository UserRepository { get; set; }
        [Inject]
        public required IRoleRepository RoleRepository { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [Inject]
        public required IDialogService DialogService { get; set; }

        public string SearchText = string.Empty;
        public string searchText
        {
            get => SearchText;
            set
            {
                SearchText = value;
                currentPage = 1;
                UpdatePaged();
            }
        }
        public int CurrentPage = 1;
        public int currentPage
        {
            get => CurrentPage;
            set
            {
                if (value > 0 && value <= (totalItems + PageSize - 1) / PageSize)
                {
                    CurrentPage = value;
                    UpdatePaged();
                }
            }
        }
        public int PageSize = 5;
        public int pageSize
        {
            get => PageSize;
            set
            {
                if (value != PageSize)
                {
                    PageSize = value;
                    CurrentPage = 1;
                    UpdatePaged();
                }
            }
        }
        public int totalItems;
        public List<object> items { get; set; } = new();

        private string GetItemTitle(object item)
        {
            return item switch
            {
                BookSale bookSale => bookSale.Title,
                Author author => author.AuthorName,
                User user => user.UserName,
                _ => string.Empty
            };
        }
        public List<object> filteredItems =>
            items.Where(item => string.IsNullOrEmpty(searchText) ||
            GetItemTitle(item).Unidecode().Contains(searchText.Unidecode(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        public List<object> pagedItems =>
            filteredItems
                .Skip((CurrentPage - 1) * pageSize)
                .Take(PageSize)
                .ToList();

        public bool IsModalVisible { get; set; }
        public Bill? SelectedBill { get; set; }
        public User? SelectedUser { get; set; }
        public async Task ShowBillDetails(int billId)
        {
            SelectedBill = await BillRepository.GetAllBillDetailsByBillId(billId);
            IsModalVisible = true;
        }

        public async Task ShowChangeRole(int userId)
        {
            SelectedUser = await UserRepository.GetUserById(userId);
            IsModalVisible = true;
        }

        public void CloseModal(bool isVisible)
        {
            IsModalVisible = isVisible;
        }
        protected override async Task OnInitializedAsync()
        {
            if (Type == "booksales" && AuthorId.HasValue)
            {
                var bookSales = await AuthorRepository.GetAllBookSalesFromAuthor(AuthorId.Value);
                items.AddRange(bookSales);
            }
            else if (Type == "booksales")
            {
                var bookSales = await BookSaleRepository.GetAllBookSales();
                items.AddRange(bookSales);
            }
            else if (Type == "authors")
            {
                var authors = await AuthorRepository.GetAllAuthors();
                items.AddRange(authors);
            }
            else if (Type == "bills")
            {
                var bills = await BillRepository.GetAllBills();
                items.AddRange(bills);
            }
            else if (Type == "users")
            {
                var users = await UserRepository.GetAllUsers();
                items.AddRange(users);
            }
            UpdatePaged();
        }


        public void ShowAllBookSalesFromAuthor(int id)
        {
            NavigationManager.NavigateTo($"/{Type}/{id}/booksales");
        }

        public void UpdateItem(int id)
        {
            NavigationManager.NavigateTo($"/{Type}/{id}/edit");
        }

        public void ViewItem(int id)
        {
            NavigationManager.NavigateTo($"/{Type}/{id}");
        }

        public async Task SetUserStatus(int id, bool IsActive)
        {
            bool? confirmed = await DialogService.ShowMessageBox(
                "Warning",
                $"Bạn muốn {(IsActive ? "Mở khóa" : "Khóa")} user với ID: {id}?",
                yesText: "Yes",
                cancelText: "No"
            );
            if (confirmed == true)
            {
                await UserRepository.SetUserStatus(id, IsActive);

                var user = (User)pagedItems.FirstOrDefault(item => ((User)item).Id == id);
                if (user != null)
                {
                    user.IsActive = IsActive;
                }

                StateHasChanged();
            }
        }

        public async Task HandleChangeRole((int id, int roleId) args)
        {
            int id = args.id;
            int roleId = args.roleId;

            await ChangeRole(id, roleId);

            IsModalVisible = false;
        }

        public async Task ChangeRole(int id, int roleId)
        {
            await UserRepository.ChangeRole(id, roleId);

            var role = await RoleRepository.GetRoleById(roleId);

            string roleName = role.RoleName;

            var user = (User)pagedItems.FirstOrDefault(item => ((User)item).Id == id);
            if (user != null)
            {
                user.Role.RoleName = roleName;
            }

            StateHasChanged();
        }

        public async Task DeleteItem(int id)
        {
            if (Type == "booksales")
            {
                bool? confirmed = await DialogService.ShowMessageBox(
                    "Xác nhận xóa",
                    $"Bạn muốn xóa Booksale với ID: {id}?",
                    yesText: "Xóa",
                    cancelText: "Hủy"
                ); if (confirmed == true)
                {
                    await BookSaleRepository.DeleteBookSale(id);
                    items.RemoveAll(item => (item as BookSale)?.Id == id);
                    UpdatePaged();
                }
            }
            else if (Type == "authors")
            {
                bool? confirmed = await DialogService.ShowMessageBox(
                    "Xác nhận xóa",
                    (MarkupString)$"Bạn muốn xóa Author với ID: {id}?<br />Điều này sẽ xóa toàn bộ BookSale thuộc Author này!!!",
                    yesText: "Xóa",
                    cancelText: "Hủy"
                );
                if (confirmed == true)
                {
                    await AuthorRepository.DeleteAuthor(id);
                    items.RemoveAll(item => (item as Author)?.Id == id);
                    UpdatePaged();
                }
            }
        }


        public void UpdatePaged()
        {
            totalItems = filteredItems.Count();
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdatePaged();
            }
        }

        public void NextPage()
        {
            if (CurrentPage < (totalItems + PageSize - 1) / PageSize)
            {
                CurrentPage++;
                UpdatePaged();
            }
        }
    }
}
