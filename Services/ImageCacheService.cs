using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ArmyOptimizer.Services
{
    public static class ImageCacheService
    {
        private static readonly Dictionary<string, BitmapImage> _cache = new();

        public static async Task<BitmapImage> LoadAsync(string url)
        {
            if (_cache.TryGetValue(url, out var cached))
                return cached;

            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(url);

            using var stream = new MemoryStream(bytes);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // ✅ ahora sí seguro

            _cache[url] = bitmap;

            return bitmap;
        }
    }
}
