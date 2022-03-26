#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alejof.SimpleBlog.Data.Impl
{
    public interface IJsonFileStore
    {
        string RootPath { get; }
        string FileName { get; }
    } 

    public static class JsonFileStoreExtensions
    {
        private const string DataFolder = "data";
        private static string GetFilePath(this IJsonFileStore service)
            => Path.Combine(service.RootPath, DataFolder, service.FileName);

        public static async Task<T> ReadContent<T>(this IJsonFileStore service, string defaultContent = "{}")
            where T : class
        {
            using var stream = new FileStream(service.GetFilePath(), FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            var content = await streamReader.ReadToEndAsync();
            if (string.IsNullOrEmpty(content))
                content = defaultContent;

            return JsonSerializer.Deserialize<T>(content);
        }

        public static async Task UpdateContent<T>(this IJsonFileStore service, T data)
            where T : class
        {
            using var stream = new FileStream(service.GetFilePath(), FileMode.Truncate);
            await JsonSerializer.SerializeAsync<T>(stream, data);
        }
    }
}