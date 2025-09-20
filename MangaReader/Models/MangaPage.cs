namespace MangaReader.Models;

public class MangaPage
{
    public string Path { get; }
    public int PageNumber { get; }

    // Конструктор тоже должен быть публичным
    public MangaPage(string path, int pageNumber)
    {
        Path = path;
        PageNumber = pageNumber;
    }
}