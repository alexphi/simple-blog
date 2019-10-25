using System;

namespace Alejof.SimpleBlog.Models
{
    public class PostCommentViewModel
    {
        public string Slug { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public Services.Models.Comment AsModel() => new Services.Models.Comment
        {
            Content = this.Content,
            Author = this.Author,
            Email = this.Email,
            Date = DateTime.Now,
        };
    }
}