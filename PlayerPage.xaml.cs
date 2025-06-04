using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using tplayer.ViewModel;

namespace tplayer;

public partial class PlayerPage : ContentPage
{
	private PlayerViewModel ViewModel => BindingContext as PlayerViewModel;

	public PlayerPage()
	{
		InitializeComponent();
		if (ViewModel != null)
		{
			ViewModel.SetMediaElement(MediaPlayer);
		}
	}

	private void OnMediaOpened(object sender, MediaOpenedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.Duration = e.Duration;
			ViewModel.IsLoading = false;
		}
	}

	private void OnMediaFailed(object sender, MediaFailedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.ErrorMessage = "Error playing media: " + e.ErrorMessage;
			ViewModel.IsLoading = false;
		}
	}

	private void OnMediaEnded(object sender, MediaEndedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.Position = TimeSpan.Zero;
			ViewModel.PlayPauseIcon = "▶";
		}
	}
}