using Plugin.Maui.Audio;
using ThunderMediaPlayer.Src.Controls;

namespace ThunderMediaPlayer.Src;

public partial class MainPage : ContentPage
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer? _player;
    private IDispatcherTimer? _timer;
    private string? _localAudioPath;

    private const string AudioUrl =
        "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3";

    public MainPage(IAudioManager audioManager)
    {
        //InitializeComponent();

        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            // Esto imprimirá el error real en la consola de VS Code
            Console.WriteLine("********* ERROR DE XAML *********");
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
                Console.WriteLine(ex.InnerException.Message);
            Console.WriteLine("*********************************");
            throw;
        }

        _audioManager = audioManager;

        cTrackBar.SeekRequested += seconds =>
        {
            _player?.Seek(seconds);
        };

        volumeControl.VolumeChanged += value =>
        {
            if (_player != null)
                _player.Volume = value;
        };

    }

    // 🔹 Timer para actualizar TrackBar
    void StartTimer()
    {
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(500);
        _timer.Tick += (_, _) =>
        {
            if (_player == null)
                return;

            cTrackBar.Duration = _player.Duration;
            cTrackBar.Position = _player.CurrentPosition;

        };
        _timer.Start();
    }


    // 🔹 Play streaming
    private async void OnPlayClicked(object sender, EventArgs e)
    {
        try
        {
            _player?.Stop();
            _player?.Dispose();

            using var http = new HttpClient();

            var tempFile = Path.Combine(
                FileSystem.CacheDirectory,
                "stream.mp3"
            );

            var bytes = await http.GetByteArrayAsync(AudioUrl);
            await File.WriteAllBytesAsync(tempFile, bytes);

            var stream = File.OpenRead(tempFile);
            _player = _audioManager.CreatePlayer(stream);
            _player.Play();

            StartTimer();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // 🔹 Play local
    private void OnLocalPlayClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_localAudioPath))
            return;

        _player?.Stop();
        _player?.Dispose();

        var stream = File.OpenRead(_localAudioPath);
        _player = _audioManager.CreatePlayer(stream);
        _player.Volume = 1.0; // volumen inicial
        volumeControl.SetVolume(_player.Volume);

        _player.Play();

        StartTimer();
    }

    // 🔹 Pick audio
    private async void OnPickAudioClicked(object sender, EventArgs e)
    {
        var audioFileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "audio/*" } },
            { DevicePlatform.iOS, new[] { "public.audio" } },
            { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".flac", ".m4a" } },
            { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
        });

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecciona un audio",
            FileTypes = audioFileTypes
        });

        if (result == null)
            return;

        _localAudioPath = Path.Combine(
            FileSystem.CacheDirectory,
            result.FileName
        );

        using var source = await result.OpenReadAsync();
        using var destination = File.Create(_localAudioPath);
        await source.CopyToAsync(destination);
    }

    // 🔹 Stop
    private void OnStopClicked(object sender, EventArgs e)
    {
        _player?.Stop();
        _player?.Dispose();
        _player = null;

        _timer?.Stop();
        cTrackBar.Position = 0;
    }
}
