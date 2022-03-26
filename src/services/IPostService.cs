#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alejof.SimpleBlog.Services
{
    public interface IPostService
    {
        Task<IList<Data.Models.Post>> GetPosts();

        Task<Data.Models.Post?> GetPost(string slug);
        Task<(bool Success, string? Error)> SavePost(Data.Models.Post post);
        Task<(bool Success, string? Error)> AddComment(string slug, Data.Models.Comment comment);
    }
}