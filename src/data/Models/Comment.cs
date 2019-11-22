using System;

namespace Alejof.SimpleBlog.Data.Models
{
    public class Comment
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}