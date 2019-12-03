using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alejof.SimpleBlog.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Alejof.SimpleBlog.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller = default!;

        [TestInitialize]
        public void Initialize()
        {
            var posts = new List<Data.Models.Post>
            {
                new Data.Models.Post { Slug = "draft", Status = SimpleBlog.Services.PostStatus.Draft },
                new Data.Models.Post { Slug = "pending", Status = SimpleBlog.Services.PostStatus.Pending },
                new Data.Models.Post { Slug = "published", Status = SimpleBlog.Services.PostStatus.Published },
            };
            
            var service = new Mock<SimpleBlog.Services.IPostService>();

            service.Setup(m => m.GetPosts())
                .ReturnsAsync(posts);
            
            service.Setup(m => m.GetPost(It.IsAny<string>()))
                .ReturnsAsync((string slug) => posts.FirstOrDefault(p => p.Slug == slug));

            // Use standard aspnet logging framework
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<HomeController>();

            _controller = new HomeController(service.Object, logger);
        }

        [TestMethod]
        public async Task HomeController_Index_ShouldReturnAllPublishedPosts()
        {
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            var list = result?.Model as IEnumerable<Models.PostIndexViewModel>;

            Assert.IsNotNull(list);
            Assert.AreEqual(1, list!.Count());
        }

        [TestMethod]
        public async Task HomeController_Detail_WithExistingSlug_ShouldReturnPost()
        {
            // Act
            var result = await _controller.Detail("published") as ViewResult;

            // Assert
            var post = result?.Model as Models.PostDetailViewModel;

            Assert.IsNotNull(post);
            Assert.AreEqual("published", post!.Slug);
        }

        [TestMethod]
        public async Task HomeController_Detail_WithNonExistingSlug_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.Detail("whatever");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
