using Shared.Repositories;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using Client.Services;

namespace Client.Components.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Parameter]
        public bool IsAdminCreate { get; set; } = false;
        [Parameter]
        public EventCallback OnSubmit { get; set; }

        [Parameter]
        [Inject]
        public required IUserClientRepository UserRepository { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required IRoleRepository RoleRepository { get; set; }
        [Inject]
        public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public required NotificationService NotificationService { get; set; }

        public RegisterRequest registerRequest = new RegisterRequest();
        public string message = string.Empty;
        public List<Role> Roles { get; set; } = new List<Role>();

        public bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            if (!IsAdminCreate)
            {
                var user = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (user.User.Identity.IsAuthenticated)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
            try
            {
                Roles = await RoleRepository.GetAllRoles();
            }
            catch (ApplicationException ex)
            {
                message = ex.Message;
                NotificationService.ShowErrorMessage(message);
            }
            catch (Exception ex)
            {
                message = $"Có lỗi xảy ra: {ex.Message}";
                NotificationService.ShowErrorMessage(message);
            }
        }

        public async Task HandleValidSubmit()
        {
            message = string.Empty;
            isLoading = true;
            try
            {
                await UserRepository.AddUser(registerRequest);
                NotificationService.ShowSuccessMessage("Đăng ký thành công!");
                if (IsAdminCreate)
                {
                    if (OnSubmit.HasDelegate)
                    {
                        await OnSubmit.InvokeAsync();
                    }
                    return;
                }
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                NotificationService.ShowErrorMessage(message);
            }
            isLoading = false;
        }
    }
}
