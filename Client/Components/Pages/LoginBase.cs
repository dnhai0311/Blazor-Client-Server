using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Components.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public required IAuthService AuthService { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public required NotificationService NotificationService { get; set; }

        public LoginRequest loginRequest = new LoginRequest();
        public string message = string.Empty;

        public bool isLoading = false;

        public async Task HandleValidSubmit()
        {
            message = string.Empty;
            isLoading = true;
            try
            {
                await AuthService.Login(loginRequest);
                NavigationManager.NavigateTo("/");
                NotificationService.ShowSuccessMessage("Đăng nhập thành công!");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                NotificationService.ShowErrorMessage(message);
            }
            isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var user = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
            }
        }

    }
}
