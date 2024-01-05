using OutSlide.Model;

namespace OutSlide;

public partial class Szel : ContentPage
{
    private OutSlideModel _model;
    public Szel(OutSlideModel model)
	{
		InitializeComponent();
		_model = model;
	}

    private void _WindLoaded(object sender, EventArgs e)
    {
        wind.Text = "";
        wind.Text = wind.Text + " " + _model._windStregth.ToString() + " km/h";
        city.Text = _model.chosenCity.ToString();
        BackgroundColor = Color.Parse(_model._windBgColor);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _model.WindLoaded += _WindLoaded;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _model.WindLoaded -= _WindLoaded;
    }

}