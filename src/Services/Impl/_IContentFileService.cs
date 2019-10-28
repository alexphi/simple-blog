using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Alejof.SimpleBlog.Services.Impl
{
    public interface IContentFileService
    {
        IWebHostEnvironment Env { get; }
        string FileName { get; }
    } 

    public static class ContentFileServiceExtensions
    {
        public const string DataFolder = "data";
        public static string GetFilePath(this IContentFileService service) => Path.Combine(service.Env.WebRootPath, DataFolder, service.FileName);

        public static async Task<T> ReadContent<T>(this IContentFileService service, string defaultContent = "{}")
            where T : class, new()
        {
            using var stream = new FileStream(service.GetFilePath(), FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            var content = await streamReader.ReadToEndAsync();
            if (string.IsNullOrEmpty(content))
                content = defaultContent;

            return JsonSerializer.Deserialize<T>(content);
        }

        public static async Task UpdateContent<T>(this IContentFileService service, T data)
            where T : class
        {
            using var stream = new FileStream(service.GetFilePath(), FileMode.Truncate);
            await JsonSerializer.SerializeAsync<T>(stream, data);
        }
    }
}