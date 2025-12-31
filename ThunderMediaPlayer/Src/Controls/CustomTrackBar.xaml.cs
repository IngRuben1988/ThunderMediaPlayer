namespace ThunderMediaPlayer.Src.Controls;

public partial class CustomTrackBar : ContentView
{
    bool _isDragging;

    public event Action<double>? SeekRequested;

    public CustomTrackBar()
    {
        InitializeComponent();
    }

    public double Duration
    {
        set
        {
            ProgressSlider.Maximum = value;
            UpdateLabel();
        }
    }

    public double Position
    {
        set
        {
            if (_isDragging)
                return;

            ProgressSlider.Value = value;
            UpdateLabel();
        }
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_isDragging)
            SeekRequested?.Invoke(e.NewValue);
    }

    void UpdateLabel()
    {
        var current = TimeSpan.FromSeconds(ProgressSlider.Value);
        var total = TimeSpan.FromSeconds(ProgressSlider.Maximum);

        TimeLabel.Text =
            $"{current:mm\\:ss} / {total:mm\\:ss}";
    }

    void OnDragStarted(object sender, EventArgs e)
        => _isDragging = true;

    void OnDragCompleted(object sender, EventArgs e)
    {
        _isDragging = false;
        SeekRequested?.Invoke(ProgressSlider.Value);
    }
}
