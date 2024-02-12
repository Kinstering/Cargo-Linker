using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Xamarin.Essentials;

namespace CargoLinker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

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

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async void RegisterUser(object sender, EventArgs e)
        {
            string name = nameField.Text;
            string password = passwordField.Text;
            string email = emailField.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Fill in all fields", "Ok");
                return;
            }

            name = name.Trim();
            password = password.Trim();
            email = email.Trim();

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format", "Ok");
                return;
            }

            if (UserService.DoesEmailExist(email))
            {
                await DisplayAlert("Error", "User with this email already exists", "Ok");
                return;
            }

            // Если дошли до этой точки, данные введены корректно
            // Добавьте код для сохранения данных пользователя в базе данных или хранилище
            User user = new User
            {
                Username = name,
                Password = password,
                Email = email
            };

            // Здесь вызывайте метод или сервис для сохранения данных пользователя
            int rowsAffected = UserService.RegisterUser(user);

            if (rowsAffected > 0)
            {
                await DisplayAlert("Success", "User registered successfully", "Ok");
                // Очистить поля ввода после успешной регистрации
                nameField.Text = passwordField.Text = emailField.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Failed to register user", "Ok");
            }
        }

        private async void GoToLoginPage(object sender, EventArgs e)
        {
            LoginPage lgPage = new LoginPage();

            NavigationPage.SetHasBackButton(lgPage, false);

            await Navigation.PushAsync(lgPage);
        }

    }
}
