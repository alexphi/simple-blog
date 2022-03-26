#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alejof.SimpleBlog.Data;
using Alejof.SimpleBlog.Data.Models;

namespace Alejof.SimpleBlog.Services.Impl
{
    public class StorePostService : IPostService
    {
        private readonly Data.IPostStore _postStore;

        public StorePostService(Data.IPostStore postStore)
        {
            this._postStore = postStore;
        }

        public Task<IList<Post>> GetPosts() => this._postStore.GetPosts();

        public Task<Post?> GetPost(string slug) => this._postStore.GetPost(slug);

        public async Task<(bool Success, string? Error)> SavePost(Post post)
        {
            Func<Post, Task<bool>> saveAction = p => this._postStore.CreatePost(p);

            var postToSave = await this._postStore.GetPost(post.Slug);
            if (postToSave == null)
            {
                postToSave = post;
            }
            else
            {
                // Merge and update
                postToSave.Author = post.Author ?? postToSave.Author;
                postToSave.Comments = post.Comments ?? postToSave.Comments;
                postToSave.Content = post.Content ?? postToSave.Content;
                postToSave.Status = post.Status ?? postToSave.Status;
                postToSave.Title = post.Title ?? postToSave.Title;
                
                saveAction = p => this._postStore.UpdatePost(p);
            }

            postToSave.UpdatedDate = DateTime.Now;
            await saveAction(postToSave);

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> AddComment(string slug, Comment comment)
        {
            var post = await this._postStore.GetPost(slug);
            if (post == null)
                return (false, "Post not found");

            post.Comments ??= new List<Comment>();
            post.Comments.Add(comment);
            await this._postStore.UpdatePost(post);

            return (true, null);
        }
    }
}