namespace ThunderMediaPlayer.Src.Controls;

public partial class CustomButton : ContentView
{
     public event EventHandler? Clicked;
    public CustomButton()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CustomButton),
            "Botón",
            propertyChanged: OnTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomButton)bindable;
        control.TextLabel.Text = (string)newValue;
    }



    private void OnTapped(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }


}
