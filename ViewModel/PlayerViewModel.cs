using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace tplayer.ViewModel
{
    public partial class PlayerViewModel : ObservableObject
    {
        private IDispatcherTimer _timer;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string playPauseIcon = "▶";

        [ObservableProperty]
        private string volumeIcon = "🔊";

        [ObservableProperty]
        private string timeDisplay = "00:00 / 00:00";

        [ObservableProperty]
        private TimeSpan position;

        [ObservableProperty]
        private TimeSpan duration;

        [ObservableProperty]
        private int volume = 100;

        [ObservableProperty]
        private MediaSource currentMedia;

        public PlayerViewModel()
        {
            SetupTimer();
        }

        private void SetupTimer()
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            if (Duration > TimeSpan.Zero)
            {
                TimeDisplay = $"{Position:mm\\:ss} / {Duration:mm\\:ss}";
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Volume))
            {
                VolumeIcon = Volume == 0 ? "🔇" : Volume < 50 ? "🔉" : "🔊";
            }
        }

        [RelayCommand]
        private async Task PlayPause()
        {
            if (CurrentMedia == null)
            {
                await OpenFile();
            }
            else
            {
                // This will be handled in the code-behind through the MediaElement
                PlayPauseIcon = PlayPauseIcon == "▶" ? "⏸" : "▶";
            }
        }

        private async Task OpenFile()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Videos
                });

                if (result != null)
                {
                    IsLoading = true;
                    CurrentMedia = MediaSource.FromFile(result.FullPath);
                    PlayPauseIcon = "⏸";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to open file";
                System.Diagnostics.Debug.WriteLine($"File open error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void OnMediaStateChanged(MediaStateChangedEventArgs args)
        {
            switch (args.NewState)
            {
                case MediaElementState.Playing:
                    PlayPauseIcon = "⏸";
                    IsLoading = false;
                    break;
                case MediaElementState.Paused:
                    PlayPauseIcon = "▶";
                    break;
                case MediaElementState.Stopped:
                    PlayPauseIcon = "▶";
                    Position = TimeSpan.Zero;
                    break;
                case MediaElementState.Failed:
                    ErrorMessage = "Error playing media";
                    IsLoading = false;
                    break;
            }
        }
    }
}
