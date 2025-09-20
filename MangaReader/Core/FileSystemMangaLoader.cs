using MangaReader.Abstractions;
using MangaReader.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MangaReader.Core
{
    public class FileSystemMangaLoader : IMangaLoader
    {
        private static readonly string[] supportedExt = { ".jpg", ".jpeg", ".png", ".bmp" };
        
        /// <summary>
        /// Возвращает MangaVolume - класс, содержащий массив путей к страницам загруженного тома и название
        /// </summary>

        public Task<MangaVolume>? LoadFromAsync(string path)
        {
            if (!Directory.Exists(path))
            {
                return Task.FromResult<MangaVolume>(null);
            }

            var pageFiles = Directory.GetFiles(path)
                .Where(file => supportedExt.Contains(Path.GetExtension(file).ToLower()))
                .OrderBy(file => file)
                .ToList();

            if (pageFiles.Count == 0)
            {
                return Task.FromResult<MangaVolume>(null);
            }
            
            var pages = new List<MangaPage>();
            for (int i = 0; i < pageFiles.Count; i++)
            {
                pages.Add(new MangaPage(pageFiles[i], i + 1));
            }
            
            var title = new DirectoryInfo(path).Name;
            var volume = new MangaVolume(title, pages);
            
            return Task.FromResult<MangaVolume>(volume);
        }
    }
}