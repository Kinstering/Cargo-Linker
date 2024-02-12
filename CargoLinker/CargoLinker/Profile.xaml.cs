using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CargoLinker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Profile : ContentPage
	{
		public Profile ()
		{
			InitializeComponent ();

            string loggedInUsername = Preferences.Get("LoggedInUsername", string.Empty);

            if (!string.IsNullOrWhiteSpace(loggedInUsername))
            {
                // Пример: отобразить имя пользователя в метке
                username.Text = loggedInUsername;
            }
        }

        private static UserService _userService;
        public static UserService UserService
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.sqlite3"));
                return _userService;
            }
        }

        private async void OnDeleteAccount(object sender, EventArgs e)
        {
            // Запросить подтверждение перед удалением аккаунта
            bool isUserConfirmed = await DisplayAlert("Confirmation", "Are you sure you want to delete your account?", "Yes", "No");

            if (isUserConfirmed)
            {
                string loggedInUsername = Preferences.Get("LoggedInUsername", string.Empty);

                if (!string.IsNullOrWhiteSpace(loggedInUsername))
                {
                    // Получите пользователя по имени (или каким-то другим уникальным идентификатором)
                    User userToDelete = UserService.GetUserByUsername(loggedInUsername);

                    // Проверка на null, чтобы избежать ошибок
                    if (userToDelete != null)
                    {
                        // Удалить пользователя из базы данных
                        UserService.DeleteUser(userToDelete);

                        // После удаления пользователя, переход на страницу входа
                        LoginPage loginPage = new LoginPage();
                        await Navigation.PushAsync(loginPage);
                    }
                }
            }
        }


        private async void OnExit(object sender, EventArgs e)
        {
            Preferences.Remove("IsLoggedIn");

            LoginPage loginPage = new LoginPage();

            await Navigation.PushAsync(loginPage);
        }

        private async void OnChangePassword(object sender, EventArgs e)
        {
            ChangePasswordPage changePassword = new ChangePasswordPage();

            await Navigation.PushAsync(changePassword);
        }

    }
}