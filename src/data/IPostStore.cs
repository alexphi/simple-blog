#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alejof.SimpleBlog.Data
{
    public interface IPostStore
    {
        Task<IList<Models.Post>> GetPosts();
        Task<Models.Post?> GetPost(string slug);

        Task<bool> CreatePost(Models.Post post);
        Task<bool> UpdatePost(Models.Post post);
        Task<bool> DeletePost(string slug);
    }
}
