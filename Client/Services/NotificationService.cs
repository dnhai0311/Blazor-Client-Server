using MudBlazor;

namespace Client.Services
{
    public class NotificationService
    {
        private readonly ISnackbar Snackbar;

        public NotificationService(ISnackbar snackbar)
        {
            Snackbar = snackbar;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
        }

        public void ShowErrorMessage(string errorMessage)
        {
            Snackbar.Add(errorMessage, Severity.Error);
        }

        public void ShowSuccessMessage(string successMessage)
        {
            Snackbar.Add(successMessage, Severity.Success);
        }
    }
}
