using CommunityToolkit.Maui.Storage;

namespace ChaplinStudio1
{
    public partial class MainPage : ContentPage
    {
        IFileSaver fileSaver;
        IFilePicker filePicker;
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(IFileSaver fileSaver)
        {
            InitializeComponent();
            this.fileSaver = fileSaver;
        }
        public MainPage(IFilePicker file1)
        {
            InitializeComponent();
            filePicker = file1;
        }
        public async Task<bool> deleteElem(string tt)
        {
            bool b = await Application.Current.MainPage.DisplayAlert("Warning", $"Delete this {tt}?", "Yes", "No");
            return b;
        }
    }
}
