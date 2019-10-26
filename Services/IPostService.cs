using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alejof.SimpleBlog.Services
{
    public interface IPostService
    {
        Task<List<Models.Post>> GetPosts();

        Task<Models.Post> GetPost(string slug);
        Task<(bool Success, string Error)> SavePost(Models.Post post);
        Task<(bool Success, string Error)> AddComment(string slug, Models.Comment comment);
    }
}