using OutSlide.Persistence;
using Newtonsoft.Json;

namespace OutSlide.Model
{
    public class OutSlideModel
    {
        private OutSlideJSON? _persistence;

        public double? _temperature;
        public double? _windStregth;
        public double? pres;
        public double? humid; 
        public string? _rain;
        public string? _windBgColor;
        public string? _rainBgColor;
        public string chosenCity = "Budapest";

        public event EventHandler? TemperatureLoaded;
        public event EventHandler? WindLoaded;
        public event EventHandler? RainLoaded;

        private Stream? _imageStream;

        public OutSlideModel(OutSlideJSON? persistence)
        {
            _persistence = persistence;
        }

        public async Task GetApiResponse(string c)
        {
            
            Uri uri = new($"https://api.openweathermap.org/data/2.5/weather?q={c}&appid=c04047f7417a3fb61689b24a4dfd5aeb&units=metric");
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                chosenCity = c;
                Root weatherResponse = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
                _temperature = weatherResponse.main.temp;
                _windStregth = weatherResponse.wind.speed;
                pres = weatherResponse.main.pressure;
                humid = weatherResponse.main.humidity;
            } else
            {
                chosenCity = "Hiba";
            }
        }
        public async Task GetTemperature(string c)
        {
            await GetApiResponse(c);
            TemperatureLoaded?.Invoke(this, EventArgs.Empty);
        }
        public async Task GetWindStrength(string c)
        {
            await GetApiResponse(c);
            if (_windStregth < 8)
            {
                _windBgColor = "Green";
            } else if (_windStregth < 16)
            {
                _windBgColor = "Yellow";
            }
            else
            {
                _windBgColor = "Red";
            }
            WindLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task GetRain(string c)
        {
            await GetApiResponse(c);
            if (pres < 800)
            {
                if (humid > 80)
                {
                    _rain = "Eső nagyon valószínű.";
                    _rainBgColor = "Red";
                } else if (humid > 60)
                {
                    _rain = "Eső valószínű.";
                    _rainBgColor = "Yellow";
                } else 
                {
                    _rain = "Eső nem valószínű.";
                    _rainBgColor = "Green";
                }
            } else if (pres < 1000)
            {
                _rain = "Eső valószínű.";
                _rainBgColor = "Yellow";
            } else
            {
                if (humid > 90)
                {
                    _rain = "Eső valószínű.";
                    _rainBgColor = "Yellow";
                }
                else
                {
                    _rain = "Eső nem valószínű.";
                    _rainBgColor = "Green";
                }
            }
            RainLoaded?.Invoke(this, EventArgs.Empty);
        }
        public async Task SaveGameAsync()
        {
            if (_persistence != null)
                await _persistence.SaveGameStateAsync(new OutSlidePersistence()
                {
                    SearchedCity = chosenCity
                });
        }
        public async Task LoadGameAsync()
        {
            if (_persistence != null)
            {
                OutSlidePersistence? state = await _persistence.LoadGameStateAsync();
                if (state != null)
                {
                    if (state.SearchedCity != null)
                    {
                        chosenCity = state.SearchedCity;
                        await GetTemperature(chosenCity);
                    }
                }
            }
        }

        public async Task SaveImage(Stream imageStream)
        {
            if (_imageStream != null)
            {
                _imageStream.Dispose();
                _imageStream = null;
            }
            _imageStream = imageStream;

            string fileName = $"photo_{DateTime.Now:yyyyMMddHHmmss}.jpg";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            using (FileStream fileStream = File.Create(filePath))
            {
                await _imageStream.CopyToAsync(fileStream);
            }
        }
    }
}