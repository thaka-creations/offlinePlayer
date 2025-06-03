
using CommunityToolkit.Mvvm.ComponentModel;

namespace tplayer.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string text;

        [ICommand]
        void Add()
        {
            Text = string.Empty;
        }
    }
}
