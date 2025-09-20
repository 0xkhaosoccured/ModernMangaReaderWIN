using System.Collections.Generic;

namespace MangaReader.Models;

public class MangaVolume
{
    public string Title { get; }
    public IReadOnlyList<MangaPage> Pages { get; }

    public MangaVolume(string title, IReadOnlyList<MangaPage> pages)
    {
        Title = title;
        Pages = pages;
    }
}