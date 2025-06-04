using tplayer.ViewModel;

namespace tplayer;

public partial class PlayerPage : ContentPage
{
	private PlayerViewModel ViewModel => BindingContext as PlayerViewModel;

	public PlayerPage()
	{
		InitializeComponent();
	}

	private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.OnMediaStateChanged(e);
		}
	}
}