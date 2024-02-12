using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace CargoLinker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();

            CargoIcon.Source = ImageSource.FromResource("CargoLinker.Resources.CargoIcon.png");

            // Перевірка чи користувач вже входив
            bool userLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (userLoggedIn)
            {
                // Якщо користувач вже входив, переходьте безпосередньо на StartPage
                Navigation.PushAsync(new StartPage());
            }

            NavigationPage.SetHasBackButton(this, false);
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

        private async void GoToRegistrationPage(object sender, EventArgs e)
        {
            MainPage regPage = new MainPage();

            // Здесь выполняйте переход на другую страницу
            // await Navigation.PushAsync(new LoginPage());

            NavigationPage.SetHasBackButton(regPage, false);

            await Navigation.PushAsync(regPage);
        }

        private async void LoginUser(object sender, EventArgs e)
        {
            string email = emailField.Text.Trim();
            string password = passwordField.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Fill in all fields", "Ok");
                return;
            }

            bool isAuthenticated = UserService.AuthenticateUser(email, password);
            string authenticatedUsername = UserService.GetUsernameByEmailAndPassword(email, password);

            if (isAuthenticated)
            {
                Preferences.Set("IsLoggedIn", true);
                // Сохранить имя пользователя в Preferences при успешной аутентификации
                Preferences.Set("LoggedInUsername", authenticatedUsername);

                await Navigation.PushAsync(new StartPage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid email or password", "Ok");
            }
        }

    }
}