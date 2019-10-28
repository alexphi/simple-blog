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
    public class HomeController : Controller
    {
        private readonly Services.IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
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
                    .Where(p => p.Status == Services.Models.PostStatus.Published)
                    .OrderByDescending(c => c.UpdatedDate)
                    .Select(p => Models.PostIndexViewModel.FromModel(p)));
        }

        [HttpGet, Route("post/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            return View(
                PostDetailViewModel.FromModel(post));
        }

        [HttpPost, Route("post/{slug?}/comment")]
        public async Task<IActionResult> PostComment([FromRoute]string slug, [FromForm]PostCommentViewModel commentViewModel)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var post = await _postService.GetPost(slug);
                if (post == null) return NotFound();
            }

            var result = await _postService.AddComment(slug, commentViewModel.AsModel());
            if (!result.Success)
            {
                // TODO: Show result.Error;
            }

            return Redirect($"/post/{slug}");
        }
    }
}
