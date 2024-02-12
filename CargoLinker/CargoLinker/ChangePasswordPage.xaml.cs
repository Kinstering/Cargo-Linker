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
	public partial class ChangePasswordPage : ContentPage
	{
		public ChangePasswordPage ()
		{
			InitializeComponent ();

            CargoIcon.Source = ImageSource.FromResource("CargoLinker.Resources.CargoIcon.png");
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

        private async void OnConfirmPassword(object sender, EventArgs e)
        {
            string username = Preferences.Get("LoggedInUsername", string.Empty);
            string currentPassword = previousPasswordEntry.Text;
            string newPassword = newPasswordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            // Получите пользователя по имени (или каким-то другим уникальным идентификатором)
            var userToUpdate = UserService.GetUserByUsername(username);

            // Проверка на null, чтобы избежать ошибок
            if (userToUpdate != null && userToUpdate.Password == currentPassword && newPassword == confirmPassword)
            {
                // Обновите пароль пользователя
                userToUpdate.Password = newPassword;

                // Сохраните изменения в базе данных
                UserService.UpdateUser(userToUpdate);

                await this.DisplayAlert("Success", "Password changed successfully", "OK");

                // Вернитесь на предыдущую страницу после успешной смены пароля.
                await Navigation.PopAsync();
            }
            else
            {
                await this.DisplayAlert("Error", "Invalid current password", "OK");
            }
        }



    }
}