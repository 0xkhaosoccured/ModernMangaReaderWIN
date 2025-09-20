// В файле Core/MangaSession.cs

using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using MangaReader.Abstractions;
using MangaReader.Models;
using System.IO;
using System.Threading.Tasks;

namespace MangaReader.Core
{
    public partial class MangaSession : ObservableObject, IMangaSession
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentPage))]
        [NotifyPropertyChangedFor(nameof(TotalPages))]
        [NotifyPropertyChangedFor(nameof(CanGoNext))]
        [NotifyPropertyChangedFor(nameof(CanGoPrevious))]
        private MangaVolume? _currentVolume;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanGoNext))]
        [NotifyPropertyChangedFor(nameof(CanGoPrevious))]
        private int _currentPageIndex = -1;

        [ObservableProperty]
        private Bitmap? _currentImage;
        
        public MangaPage? CurrentPage => CurrentVolume is not null && CurrentPageIndex >= 0 ? CurrentVolume.Pages[CurrentPageIndex] : null;
        public int TotalPages => CurrentVolume?.Pages.Count ?? 0;
        public bool CanGoNext => CurrentVolume is not null && CurrentPageIndex < TotalPages - 1;
        public bool CanGoPrevious => CurrentVolume is not null && CurrentPageIndex > 0;
        
        
        public async Task OpenVolumeAsync(MangaVolume volume)
        {
            CurrentVolume = volume;
            _currentPageIndex = -1; 
            await GoToPageAsync(0);
        }
        
        public async Task GoToPageAsync(int pageIndex)
        {
            if (CurrentVolume is null || pageIndex < 0 || pageIndex >= TotalPages || pageIndex == CurrentPageIndex)
                return;

            CurrentPageIndex = pageIndex;
            
            var page = CurrentVolume.Pages[pageIndex];
            await using var stream = File.OpenRead(page.Path);
            CurrentImage?.Dispose(); 
            CurrentImage = new Bitmap(stream);
        }

        public async Task GoToNextPageAsync()
        {
            if (CanGoNext) await GoToPageAsync(CurrentPageIndex + 1);
        }

        public async Task GoToPreviousPageAsync()
        {
            if (CanGoPrevious) await GoToPageAsync(CurrentPageIndex - 1);
        }

        public void CloseVolume()
        {
            CurrentVolume = null;
            CurrentImage?.Dispose();
            CurrentImage = null;
            CurrentPageIndex = -1;
        }
    }
}