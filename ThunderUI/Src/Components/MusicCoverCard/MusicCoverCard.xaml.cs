namespace ThunderUI.Src.Components.MusicCoverCard; // O la ruta exacta que tengas

public partial class MusicCoverCard : ContentView
{
    public MusicCoverCard()
    {
        //InitializeComponent();
        //BindingContext = this;
        //SetDefaultImage();
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

    }

    /* ======================
       Cover Image
       ====================== */

    public static readonly BindableProperty CoverSourceProperty =
        BindableProperty.Create(
            nameof(CoverSource),
            typeof(ImageSource),
            typeof(MusicCoverCard),
            null,
            propertyChanged: OnCoverChanged);

    public ImageSource? CoverSource
    {
        get => (ImageSource?)GetValue(CoverSourceProperty);
        set => SetValue(CoverSourceProperty, value);
    }

    static void OnCoverChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (MusicCoverCard)bindable;

        // ⛔ El control aún no está listo
        if (control.CoverImage == null)
            return;

        if (newValue == null)
        {
            control.CoverImage.Source = null;
            return;
        }

        // ✔ Ya es ImageSource
        if (newValue is ImageSource source)
        {
            control.CoverImage.Source = source;
            return;
        }

        // ✔ Viene como string desde XAML
        if (newValue is string fileName)
        {
            control.CoverImage.Source = ImageSource.FromFile(fileName);
            return;
        }
    }



    /* ======================
       Title
       ====================== */

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(MusicCoverCard),
            string.Empty,
            propertyChanged: (b, _, n) =>
            {
                ((MusicCoverCard)b).TitleLabel.Text = (string)n;
            });

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /* ======================
       Artist
       ====================== */

    public static readonly BindableProperty ArtistProperty =
        BindableProperty.Create(
            nameof(Artist),
            typeof(string),
            typeof(MusicCoverCard),
            string.Empty,
            propertyChanged: (b, _, n) =>
            {
                ((MusicCoverCard)b).ArtistLabel.Text = (string)n;
            });

    public string Artist
    {
        get => (string)GetValue(ArtistProperty);
        set => SetValue(ArtistProperty, value);
    }

    /* ======================
       Overlay
       ====================== */

    public static readonly BindableProperty ShowOverlayProperty =
        BindableProperty.Create(
            nameof(ShowOverlay),
            typeof(bool),
            typeof(MusicCoverCard),
            false);

    public bool ShowOverlay
    {
        get => (bool)GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    /* ======================
       Helpers
       ====================== */

    void SetDefaultImage()
    {
        CoverImage.Source = ImageSource.FromFile("vinilo.png");
    }
}
