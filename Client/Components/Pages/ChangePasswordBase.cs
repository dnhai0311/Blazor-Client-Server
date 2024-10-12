using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Shared.Models;
using Shared.Repositories;
using System.Security.Claims;

namespace Client.Components.Pages
{
    public class ChangePasswordBase : ComponentBase
    {
        [Inject]
        public required IUserClientRepository UserRepository { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [Inject]
        public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest();
        public string message = string.Empty;

        public async Task HandleValidSubmit()
        {
            message = string.Empty;
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                int userId = Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await UserRepository.ChangePassword(userId, changePasswordRequest);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
    }
}
