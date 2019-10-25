using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Alejof.SimpleBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alejof.SimpleBlog.Controllers
{
    public class EditorController : Controller
    {
        private readonly Services.IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public EditorController(
            Services.IPostService postService,
            ILogger<HomeController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPosts();

            return View(
                posts
                    .Where(p => p.Status == Services.Models.PostStatus.Pending)
                    .OrderByDescending(c => c.UpdatedDate)
                    .Select(p => Models.PostIndexViewModel.FromModel(p)));
        }
    }
}
