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
    public class ApiControllerTests
    {
        private EditorApiController _controller = default!;

        [TestInitialize]
        public void Initialize()
        {
            var posts = new List<Data.Models.Post>
            {
                new Data.Models.Post { Slug = "pending", Status = SimpleBlog.Services.PostStatus.Pending },
                new Data.Models.Post { Slug = "error", Status = SimpleBlog.Services.PostStatus.Pending },
                new Data.Models.Post { Slug = "published", Status = SimpleBlog.Services.PostStatus.Published },
            };
            
            var service = new Mock<SimpleBlog.Services.IPostService>();

            service.Setup(m => m.GetPosts())
                .ReturnsAsync(posts);
            
            service.Setup(m => m.GetPost(It.IsAny<string>()))
                .ReturnsAsync((string slug) => posts.FirstOrDefault(p => p.Slug == slug));

            service.Setup(m => m.SavePost(It.IsAny<Data.Models.Post>()))
                .ReturnsAsync((Data.Models.Post post) =>
                    post.Slug == "error" ? (false, "error") : (true, null));

            // Use standard aspnet logging framework
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<EditorApiController>();

            _controller = new EditorApiController(service.Object, logger);
        }

        [TestMethod]
        public async Task EditorApiController_Index_ShouldReturnOkWithPendingPosts()
        {
            // Act
            var result = await _controller.Index() as OkObjectResult;

            // Assert
            var list = result?.Value as IEnumerable<Models.PostEditViewModel>;

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list!.Count());
        }

        [TestMethod]
        public async Task EditorApiController_Approve_WithPendingSlug_ShouldReturnOk()
        {
            // Act
            var result = await _controller.Approve("pending");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task EditorApiController_Approve_WithNonPendingSlug_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.Approve("published");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task EditorApiController_Approve_WithNonExistingSlug_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.Approve("whatever");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task EditorApiController_Approve_GivenServiceError_ShouldReturnUnprocessable()
        {
            // Act
            var result = await _controller.Approve("error");

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnprocessableEntityObjectResult));
        }
    }
}
