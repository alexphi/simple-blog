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
    [Route("api/v1/")]
    public class EditorApiController : Controller
    {
        private readonly Services.IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public EditorApiController(
            Services.IPostService postService,
            ILogger<HomeController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet, Route("posts")]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPosts();

            return Ok(
                posts
                    .Where(p => p.Status == Services.Models.PostStatus.Pending)
                    .OrderByDescending(c => c.UpdatedDate)
                    .Select(p => Models.PostEditViewModel.FromModel(p)));
        }

        [HttpPost, Route("posts/{slug?}/approve")]
        public async Task<IActionResult> PostApprove([FromRoute]string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            post.Status = Services.Models.PostStatus.Published;
            post.ApprovalDate = DateTime.Now;

            var result = await _postService.SavePost(post);
            if (!result.Success)
                return this.UnprocessableEntity();

            return Ok();
        }

        [HttpPost, Route("posts/{slug?}/reject")]
        public async Task<IActionResult> PostReject([FromRoute]string slug)
        {
            var post = await _postService.GetPost(slug);
            if (post == null) return NotFound();

            post.Status = Services.Models.PostStatus.Draft;

            var result = await _postService.SavePost(post);
            if (!result.Success)
                return this.UnprocessableEntity();

            return Ok();
        }
    }
}
