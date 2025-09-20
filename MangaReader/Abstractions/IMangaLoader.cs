using MangaReader.Models;
using System.Threading.Tasks;
namespace MangaReader.Abstractions;

public interface IMangaLoader
{
    Task<MangaVolume>? LoadFromAsync(string path);
}