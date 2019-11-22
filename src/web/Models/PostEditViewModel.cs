using System;

namespace Alejof.SimpleBlog.Models
{
    public class PostEditViewModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string UpdatedDate { get; set; }

        public static PostEditViewModel FromModel(Data.Models.Post model) => new PostEditViewModel
        {
            Slug = model.Slug,
            Title = model.Title,
            Content = model.Content,
            Author = model.Author,
            UpdatedDate = model.UpdatedDate.ToString("yyyy-MM-dd"),
        };

        public Data.Models.Post AsModel() => new Data.Models.Post
        {
            Slug = this.Slug,
            Title = this.Title,
            Content = this.Content,
            Author = this.Author,
        };
    }
}