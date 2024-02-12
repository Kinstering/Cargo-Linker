using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System;

namespace CargoLinker
{
    public partial class StartPage : MasterDetailPage
    {
        private Xamarin.Forms.Maps.Map myCustomMap;

        public StartPage()
        {
            InitializeComponent();

            ImageButton menuButton = this.FindByName<ImageButton>("menuButton"); // Замените "menuButton" на имя вашей кнопки
            menuButton.Clicked += OnMenuButtonClicked;

            NavigationPage.SetHasNavigationBar(this, false);

            myCustomMap = new Xamarin.Forms.Maps.Map
            {
                IsShowingUser = true,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            // Получение текущего местоположения устройства
            GetDeviceLocationAsync();

            // Создание маркера пользователя
            Pin userPin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(37.785834, -122.406417),
                Label = "User Location",
                Address = "Current location"
            };

            myCustomMap.Pins.Add(userPin);


            string loggedInUsername = Preferences.Get("LoggedInUsername", string.Empty);

            if (!string.IsNullOrWhiteSpace(loggedInUsername))
            {
                // Пример: отобразить имя пользователя в метке
                username.Text = loggedInUsername;
            }
        }

        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            // Откройте меню при нажатии на кнопку
            IsPresented = true;
        }

        private async void GetDeviceLocationAsync()
        {
            var location = await Geolocation.GetLocationAsync();
            if (location != null)
            {
                myCustomMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude),
                    Distance.FromMiles(0.5)));
            }
        }

        private async void GoToTruckPage(object sender, EventArgs e)
        {
            TruckPage trPage = new TruckPage();

            await Navigation.PushAsync(trPage);
        }

        private async void GoToStartTripPage(object sender, EventArgs e)
        {
            StartTripPage startTripPage = new StartTripPage();

            await Navigation.PushAsync(startTripPage);
        }


        private async void OnStackLayoutTapped(object sender, EventArgs e)
        {
            // В этом методе осуществите переход на другую страницу
            await Navigation.PushAsync(new Profile());
        }

    }
}
