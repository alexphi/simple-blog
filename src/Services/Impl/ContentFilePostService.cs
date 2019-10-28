using System;
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

            post.UpdatedDate = DateTime.Now;
            posts.Add(post);

            await this.UpdateContent(posts);
            return (true, null);
        }

        public async Task<(bool Success, string Error)> AddComment(string slug, Comment comment)
        {
            var posts = await this.GetPosts();
            var post = posts
                .FirstOrDefault(p => p.Slug == slug);

            if (post == null)
                return (false, "Post not found");

            if (post.Comments == null)
                post.Comments = new List<Comment>();
                
            post.Comments.Add(comment);

            await this.UpdateContent(posts);
            return (true, null);
        }
    }
}