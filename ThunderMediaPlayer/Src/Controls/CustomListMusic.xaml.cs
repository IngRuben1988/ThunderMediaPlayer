using System.Collections.ObjectModel;
using ThunderMediaPlayer.Src.Models;

namespace ThunderMediaPlayer.Src.Controls;

public partial class CustomListMusic : ContentView
{
    public event Action<AudioItem>? TrackSelected;

    ObservableCollection<AudioItem> _audios = new();

    public CustomListMusic()
    {
        InitializeComponent();
        AudioList.ItemsSource = _audios;
    }

    async void OnPickMusicClicked(object sender, EventArgs e)
    {
        var fileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "audio/*" } },
            { DevicePlatform.iOS, new[] { "public.audio" } },
            { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".flac", ".m4a" } },
            { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
        });

        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Selecciona tu música",
            FileTypes = fileTypes
        });

        if (results == null)
            return;

        _audios.Clear();

        foreach (var file in results)
        {
            var localPath = Path.Combine(
                FileSystem.CacheDirectory,
                file.FileName
            );

            using var source = await file.OpenReadAsync();
            using var dest = File.Create(localPath);
            await source.CopyToAsync(dest);

            _audios.Add(new AudioItem
            {
                FileName = file.FileName,
                FullPath = localPath
            });
        }
    }

    void OnAudioSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not AudioItem audio)
            return;

        TrackSelected?.Invoke(audio);

        AudioList.SelectedItem = null;
    }
}
