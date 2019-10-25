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
        public async Task<IActionResult> PostEdit([FromRoute]string slug, [FromForm]PostEditViewModel postModel)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var post = await _postService.GetPost(slug);
                if (post == null) return NotFound();
            }

            var result = await _postService.SavePost(postModel.AsModel());
            if (!result.Success)
            {
                // TODO: Show result.Error;
            }

            return Redirect("/");
        }

        [HttpPost, Route("comment/{slug?}")]
        public async Task<IActionResult> PostComment([FromRoute]string slug, [FromForm]PostCommentViewModel commentModel)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var post = await _postService.GetPost(slug);
                if (post == null) return NotFound();
            }

            var result = await _postService.AddComment(slug, commentModel.AsModel());
            if (!result.Success)
            {
                // TODO: Show result.Error;
            }

            return Redirect($"/post/{slug}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
