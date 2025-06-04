using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using LibVLCSharp.Shared;
using LibVLCSharp.Platforms.MAUI;

namespace tplayer.ViewModel
{
    public partial class PlayerViewModel : ObservableObject
    {
        private IDispatcherTimer _timer;
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

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
        private string currentMedia;

        private VideoView videoView;

        public PlayerViewModel()
        {
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.Playing += (s, e) => OnPlayerStateChanged(VLCState.Playing);
            _mediaPlayer.Paused += (s, e) => OnPlayerStateChanged(VLCState.Paused);
            _mediaPlayer.Stopped += (s, e) => OnPlayerStateChanged(VLCState.Stopped);
            _mediaPlayer.EncounteredError += (s, e) => OnPlayerStateChanged(VLCState.Error);
            SetupTimer();
        }

        public void SetVideoView(VideoView view)
        {
            videoView = view;
            if (videoView != null)
            {
                videoView.MediaPlayer = _mediaPlayer;
            }
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
            if (_mediaPlayer != null && _mediaPlayer.Length > 0)
            {
                Position = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
                Duration = TimeSpan.FromMilliseconds(_mediaPlayer.Length);
                TimeDisplay = $"{Position:mm\\:ss} / {Duration:mm\\:ss}";
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Volume))
            {
                VolumeIcon = Volume == 0 ? "🔇" : Volume < 50 ? "🔉" : "🔊";
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Volume = Volume;
                }
            }
        }

        [RelayCommand]
        private async Task PlayPause()
        {
            if (string.IsNullOrEmpty(CurrentMedia))
            {
                await OpenFile();
            }
            else if (_mediaPlayer != null)
            {
                if (_mediaPlayer.IsPlaying)
                {
                    _mediaPlayer.Pause();
                    PlayPauseIcon = "▶";
                }
                else
                {
                    _mediaPlayer.Play();
                    PlayPauseIcon = "⏸";
                }
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
                    CurrentMedia = result.FullPath;
                    PlayPauseIcon = "⏸";
                    if (_mediaPlayer != null)
                    {
                        using (var media = new Media(_libVLC, result.FullPath, FromType.FromPath))
                        {
                            _mediaPlayer.Media = media;
                            _mediaPlayer.Play();
                        }
                    }
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

        private void OnPlayerStateChanged(VLCState state)
        {
            switch (state)
            {
                case VLCState.Playing:
                    PlayPauseIcon = "⏸";
                    IsLoading = false;
                    break;
                case VLCState.Paused:
                    PlayPauseIcon = "▶";
                    break;
                case VLCState.Stopped:
                    PlayPauseIcon = "▶";
                    Position = TimeSpan.Zero;
                    break;
                case VLCState.Error:
                    ErrorMessage = "Error playing media";
                    IsLoading = false;
                    break;
            }
        }
    }
}
