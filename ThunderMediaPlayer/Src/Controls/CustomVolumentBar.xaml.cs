namespace ThunderMediaPlayer.Src.Controls;

public partial class CustomVolumeBar : ContentView
{
    public event Action<double>? VolumeChanged;

    public CustomVolumeBar()
    {
        InitializeComponent();
    }

    void OnVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        VolumeChanged?.Invoke(e.NewValue);
    }


    public void SetVolume(double value)
    {
        VolumeSlider.Value = value;
    }
}
