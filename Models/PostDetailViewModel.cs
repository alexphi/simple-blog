using System;
using System.Collections.Generic;
using System.Linq;

namespace Alejof.SimpleBlog.Models
{
    public class PostDetailViewModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string UpdatedDate { get; set; }

        public IEnumerable<PostDetailCommentViewModel> Comments { get; set; }

        public static PostDetailViewModel FromModel(Services.Models.Post model) => new PostDetailViewModel
        {
            Slug = model.Slug,
            Title = model.Title,
            Content = model.Content,
            Author = model.Author,
            UpdatedDate = model.UpdatedDate.ToString("yyyy-MM-dd"),
            Comments = (model.Comments ?? Enumerable.Empty<Services.Models.Comment>())
                .OrderByDescending(c => c.Date)
                .Select(c => PostDetailCommentViewModel.FromModel(c)),
        };
    }

    public class PostDetailCommentViewModel
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }

        public static PostDetailCommentViewModel FromModel(Services.Models.Comment model) => new PostDetailCommentViewModel
        {
            Content = model.Content,
            Author = model.Author,
            Email = model.Email,
            Date = model.Date.ToString("yyyy-MM-dd")
        };
    }
}