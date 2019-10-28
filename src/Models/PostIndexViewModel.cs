using System;

namespace Alejof.SimpleBlog.Models
{
    public class PostIndexViewModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string UpdatedDate { get; set; }

        public static PostIndexViewModel FromModel(Services.Models.Post model) => new PostIndexViewModel
        {
            Slug = model.Slug,
            Title = model.Title,
            Content = model.Content?.Length > 50 ?
                (model.Content.Substring(0, 50) + "...")
                : model.Content,
            Author = model.Author,
            UpdatedDate = model.UpdatedDate.ToString("yyyy-MM-dd"),
        };
    }
}