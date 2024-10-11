﻿using Shared.Repositories;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Components.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        public required IUserClientRepository UserRepository { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required IRoleRepository RoleRepository { get; set; }

        public RegisterRequest registerRequest = new RegisterRequest();
        public string message = string.Empty;
        public List<Role> Roles { get; set; } = new List<Role>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Roles = await RoleRepository.GetAllRoles();
            }
            catch (ApplicationException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = $"Có lỗi xảy ra: {ex.Message}";
            }
        }

        public async Task HandleValidSubmit()
        {
            try
            {
                await UserRepository.AddUser(registerRequest);
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
    }
}