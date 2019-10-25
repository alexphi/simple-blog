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
    public class WriterController : Controller
    {
        private readonly Services.IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public WriterController(
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
                    .Where(p => string.IsNullOrEmpty(p.Status) || p.Status == Services.Models.PostStatus.Draft)
                    .OrderByDescending(c => c.UpdatedDate)
                    .Select(p => Models.PostIndexViewModel.FromModel(p)));
        }

        [HttpGet, Route("edit/{slug?}")]
        public async Task<IActionResult> Edit([FromRoute]string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return View();

            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            return View(
                PostEditViewModel.FromModel(post));
        }

        [HttpPost, Route("edit/{slug?}")]
        public async Task<IActionResult> PostEdit([FromRoute]string slug, [FromForm]PostEditViewModel postViewModel)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var existingPost = await _postService.GetPost(slug);
                if (existingPost == null) return NotFound();
            }

            var post = postViewModel.AsModel();
            post.Status = Services.Models.PostStatus.Draft;

            var result = await _postService.SavePost(post);
            if (!result.Success)
            {
                // TODO: Show result.Error;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
