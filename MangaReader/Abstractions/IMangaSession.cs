using System.ComponentModel;
using System.Threading.Tasks;
using MangaReader.Models;
using Avalonia.Media.Imaging; 
using System.Threading.Tasks;

namespace MangaReader.Abstractions;

public interface IMangaSession : INotifyPropertyChanged
{
    MangaVolume? CurrentVolume { get; }
    MangaPage? CurrentPage { get; }
    Bitmap? CurrentImage { get; }
    int CurrentPageIndex { get; }
    int TotalPages { get; }
    bool CanGoNext { get; }
    bool CanGoPrevious { get; }

    Task OpenVolumeAsync(MangaVolume volume);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    void CloseVolume();
}