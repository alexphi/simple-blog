using System;
using System.Collections.Generic;

namespace Alejof.SimpleBlog.Services.Models
{
    public class Post
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<Comment> Comments { get; set; }
    }
}