using Microsoft.Maui.Controls;
using OutSlide.Model;
using OutSlide.Persistence;


namespace OutSlide
{
    public partial class App : Application
    {
        private OutSlideJSON _persistence;
        private OutSlideModel _model;
        private NavigationPage _rootPage;

        public App()
        {
            InitializeComponent();
            _persistence = new OutSlideJSON();
            _model = new OutSlideModel(_persistence);
            _rootPage = new NavigationPage(new MainPage(_model));
            MainPage = _rootPage;
        }
        protected override async void OnStart()
        {
            await _model.LoadGameAsync();
            base.OnStart();
        }
        protected override async void OnSleep()
        {
            await _model.SaveGameAsync();
            base.OnSleep();
        }
        protected override async void OnResume()
        {
            await _model.LoadGameAsync();
            base.OnResume();
        }
    }
}