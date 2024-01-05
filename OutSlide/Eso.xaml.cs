using OutSlide.Model;

namespace OutSlide;

public partial class Eso : ContentPage
{
    private OutSlideModel _model;
    public Eso(OutSlideModel model)
	{
		InitializeComponent();
        _model = model;
    }
    private void _RainLoaded(object sender, EventArgs e)
    {
        rain.Text = "";
        rain.Text = rain.Text + " " + _model._rain.ToString();
        city.Text = _model.chosenCity.ToString();
        BackgroundColor = Color.Parse(_model._rainBgColor);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _model.RainLoaded += _RainLoaded;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _model.RainLoaded -= _RainLoaded;
    }
}