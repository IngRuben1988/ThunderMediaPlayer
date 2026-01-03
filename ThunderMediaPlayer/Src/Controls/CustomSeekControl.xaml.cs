namespace ThunderMediaPlayer.Src.Controls;

public partial class CustomSeekControls : ContentView
{
    public event Action<double>? SeekRequested;
    public event Action? PlaybackRateToggleRequested;

    DateTime _lastForwardClick = DateTime.MinValue;
    DateTime _lastRewindClick = DateTime.MinValue;

    const double SeekSeconds = 10;

    static readonly TimeSpan DoubleClickThreshold =
        TimeSpan.FromMilliseconds(350);

    public CustomSeekControls()
    {
        InitializeComponent();
    }

    void OnForwardClicked(object sender, EventArgs e)
    {
        var now = DateTime.UtcNow;

        if (now - _lastForwardClick < DoubleClickThreshold)
        {
            PlaybackRateToggleRequested?.Invoke();
        }
        else
        {
            SeekRequested?.Invoke(+SeekSeconds);
        }

        _lastForwardClick = now;
    }

    void OnRewindClicked(object sender, EventArgs e)
    {
        var now = DateTime.UtcNow;

        if (now - _lastRewindClick < DoubleClickThreshold)
        {
            PlaybackRateToggleRequested?.Invoke();
        }
        else
        {
            SeekRequested?.Invoke(-SeekSeconds);
        }

        _lastRewindClick = now;
    }
}
