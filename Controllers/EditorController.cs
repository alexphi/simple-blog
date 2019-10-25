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

        [HttpGet, Route("review/{slug?}")]
        public async Task<IActionResult> Review([FromRoute]string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            return View(PostEditViewModel.FromModel(post));
        }

        [HttpPost, Route("review/{slug?}/approve")]
        public async Task<IActionResult> PostApprove([FromRoute]string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            post.Status = Services.Models.PostStatus.Published;
            post.ApprovalDate = DateTime.Now;

            var result = await _postService.SavePost(post);
            if (!result.Success)
            {
                // TODO: Show result.Error;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, Route("review/{slug?}/reject")]
        public async Task<IActionResult> PostReject([FromRoute]string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

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
