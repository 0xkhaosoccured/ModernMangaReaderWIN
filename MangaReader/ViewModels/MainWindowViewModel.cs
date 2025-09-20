using System.IO;
using Avalonia.Media.Imaging; 
using Avalonia.Platform.Storage; 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using MangaReader.Models;
using MangaReader.Core;
using MangaReader.ViewModels;
using MangaReader.Abstractions;
namespace MangaReader.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMangaLoader _mangaLoader;
    private readonly IMangaSession _mangaSession;

    public MainWindowViewModel(IMangaLoader mangaLoader, IMangaSession mangaSession)
    {
        _mangaLoader = mangaLoader;
        _mangaSession = mangaSession;
        
        _mangaSession.PropertyChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(CurrentImage));
            OnPropertyChanged(nameof(Volume_name));
            OnPropertyChanged(nameof(Page));
            
            GoToNextPageCommand.NotifyCanExecuteChanged();
            GoToPreviousPageCommand.NotifyCanExecuteChanged();
        };
    }
    
    public Bitmap? CurrentImage => _mangaSession.CurrentImage;
    public string Volume_name => _mangaSession.CurrentVolume?.Title ?? "Том не открыт";
    public string Page => _mangaSession.CurrentVolume is null ? "0 / 0" : $"{_mangaSession.CurrentPageIndex + 1} / {_mangaSession.TotalPages}";
    
    public bool CanGoNext => _mangaSession.CanGoNext;
    public bool CanGoPrevious => _mangaSession.CanGoPrevious;

    [RelayCommand(CanExecute = nameof(CanGoPrevious))]
    public async Task GoToPreviousPageAsync()
    {
        await _mangaSession.GoToPreviousPageAsync();
    }

    [RelayCommand(CanExecute = nameof(CanGoNext))]
    public async Task GoToNextPageAsync()
    {
        await _mangaSession.GoToNextPageAsync();
    }

    
    [RelayCommand]
    public async Task OpenVolumeAsync()
    {
        var topLevel = App.GetTopLevel(); 
        
        if (topLevel is null) return;
        
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку с томом манги",
            AllowMultiple = false
        });
        if (folders.Count > 0 && folders[0].Path.LocalPath is string folderPath)
        {
            var volume = await _mangaLoader.LoadFromAsync(folderPath);
            if (volume is not null)
            {

                await _mangaSession.OpenVolumeAsync(volume);
                
                OnPropertyChanged(nameof(CurrentImage));
                OnPropertyChanged(nameof(Volume_name));
                OnPropertyChanged(nameof(Page));
            }
        }
    }
}