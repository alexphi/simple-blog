using System;
using System.Collections.Generic;

namespace Alejof.SimpleBlog.Models
{
    public class PostDetailViewModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string UpdatedDate { get; set; }

        public IEnumerable<Services.Models.Comment> Comments { get; set; }

        public static PostDetailViewModel FromModel(Services.Models.Post model) => new PostDetailViewModel
        {
            Slug = model.Slug,
            Title = model.Title,
            Content = model.Content,
            Author = model.Author,
            UpdatedDate = model.UpdatedDate.ToString("yyyy-MM-dd"),
            Comments = model.Comments,
        };
    }
}