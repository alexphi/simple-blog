using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alejof.SimpleBlog.Services.Models;
using Microsoft.AspNetCore.Hosting;

namespace Alejof.SimpleBlog.Services.Impl
{
    public class ContentFilePostService : IPostService, IContentFileService
    {
        public ContentFilePostService(IWebHostEnvironment env)
        {
            this.Env = env;
        }

        public IWebHostEnvironment Env { get; private set; }
        public string FileName => "posts.json";

        public Task<List<Post>> GetPosts() => this.ReadContent<List<Post>>(defaultContent: "[]");

        public async Task<Post> GetPost(string slug)
        {
            var posts = await this.GetPosts();
            return posts
                .FirstOrDefault(p => p.Slug == slug);
        }

        public async Task<(bool Success, string Error)> SavePost(Post post)
        {
            var posts = await this.GetPosts();

            var oldPost = posts
                .FirstOrDefault(p => p.Slug == post.Slug);
            if (oldPost != null)
                posts.Remove(oldPost);

            posts.Add(post);

            await this.UpdateContent(posts);
            return (true, null);
        }
    }
}