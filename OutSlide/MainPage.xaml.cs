using OutSlide.Model;

namespace OutSlide
{
    public partial class MainPage : ContentPage
    {
        private OutSlideModel _model;
        private bool CanGoToPages = true;
        public MainPage(OutSlideModel model)
        {
            InitializeComponent();
            _model = model;
            setOnce();
        }
        private async void setOnce()
        {
            await _model.GetTemperature("Budapest");
            temp.Text = "";
            temp.Text = temp.Text + " " + _model._temperature.ToString() + " °C";
            city.Text = _model.chosenCity.ToString();
        }

        public void TempLoaded(object sender, EventArgs e)
        {
            temp.Text = "";
            if (_model.chosenCity == "Hiba")
            {
                city.Text = "Biztos jól írtad a város nevét?";
                CanGoToPages = false;
            } else
            {
                CanGoToPages = true;
                temp.Text = temp.Text + " " + _model._temperature.ToString() + " °C";
                city.Text = _model.chosenCity.ToString();
            }
            
        }
        private async void _keresesDone(object sender, EventArgs e)
        {
            await _model.GetTemperature(_kereses.Text);
            _kereses.IsEnabled = false;
            _kereses.IsEnabled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.TemperatureLoaded += TempLoaded;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _model.TemperatureLoaded -= TempLoaded;
        }
        private async void GoToPage1(object sender, EventArgs e)
        {
            if (!CanGoToPages)
            {
                return;
            }
            await Navigation.PushAsync(new Szel(_model));
            await _model.GetWindStrength(_model.chosenCity);
        }

        private async void GoToPage2(object sender, EventArgs e)
        {
            if (!CanGoToPages)
            {
                return;
            }
            await Navigation.PushAsync(new Eso(_model));
            await _model.GetRain(_model.chosenCity);
        }

        private async void TakePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                FileResult photoFile = await MediaPicker.CapturePhotoAsync();
                if (photoFile != null)
                    await _model.SaveImage(await photoFile.OpenReadAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

    }
}