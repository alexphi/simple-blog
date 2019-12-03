using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Alejof.SimpleBlog.Tests.Services
{
    [TestClass]
    public class PostServiceTests
    {
        private SimpleBlog.Services.Impl.StorePostService _service = default!;

        private readonly IList<Data.Models.Post> _storage = new List<Data.Models.Post>
        {
            new Data.Models.Post { Slug = "post1" },
            new Data.Models.Post { Slug = "post2" },
        };

        [TestInitialize]
        public void Initialize()
        {
            var storeMock = new Mock<Data.IPostStore>();
            storeMock.Setup(m => m.GetPosts())
                .ReturnsAsync(_storage);

            storeMock.Setup(m => m.GetPost(It.IsAny<string>()))
                .ReturnsAsync((string slug) => _storage.FirstOrDefault(p => p.Slug == slug));
            
            storeMock.Setup(m => m.CreatePost(It.IsAny<Data.Models.Post>()))
                .Callback((Data.Models.Post post) => _storage.Add(post))
                .ReturnsAsync(true);
            
            storeMock.Setup(m => m.UpdatePost(It.IsAny<Data.Models.Post>()))
                .ReturnsAsync(true);
            
            storeMock.Setup(m => m.DeletePost(It.IsAny<string>()))
                .ReturnsAsync(true);

            _service = new SimpleBlog.Services.Impl.StorePostService(storeMock.Object);
        }

        [TestMethod]
        public async Task PostService_GetPosts_ShouldReturnAllPosts()
        {
            // Act
            var result = await _service.GetPosts();

            // Assert
            Assert.AreEqual(_storage.Count, result.Count);
        }

        [TestMethod]
        public async Task PostService_GetPost_WithMatchingSlug_ShouldReturnPost()
        {
            // Arrange
            var post = _storage.Last();
            
            // Act
            var result = await _service.GetPost(post.Slug);

            // Assert
            Assert.AreEqual(post.Slug, result.Slug);
        }

        [TestMethod]
        public async Task PostService_GetPost_WithNonMatchingSlug_ShouldReturnNull()
        {
            // Act
            var result = await _service.GetPost("whatever");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task PostService_SavePost_WithNewSlug_ShouldCreatePostAndReturnNullMessage()
        {
            // Arrange
            var postCount = _storage.Count;

            // Act
            var (result, message) = await _service.SavePost(new Data.Models.Post { Slug = "new!" });

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(message);

            Assert.AreEqual(postCount + 1, _storage.Count);
        }

        [TestMethod]
        public async Task PostService_SavePost_WithExistingSlug_ShouldUpdatePostAndReturnNullMessage()
        {
            // Arrange
            var post = _storage.Last();
            var updatedDate = post.UpdatedDate;

            // Act
            var (result, message) = await _service.SavePost(new Data.Models.Post { Slug = post.Slug });

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(message);

            Assert.AreNotEqual(updatedDate, post.UpdatedDate);
        }

        [TestMethod]
        public async Task PostService_AddComment_WithExistingSlug_ShouldAddCommentAndReturnNullMessage()
        {
            // Arrange
            var post = _storage.Last();
            var comment = new Data.Models.Comment();

            // Act
            var (result, message) = await _service.AddComment(post.Slug, comment);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(message);

            Assert.AreSame(comment, post.Comments.First());
        }

        [TestMethod]
        public async Task PostService_AddComment_WithNonExistingSlug_ShouldReturnFalseAndMessage()
        {
            // Arrange
            var comment = new Data.Models.Comment();

            // Act
            var (result, message) = await _service.AddComment("whatever", comment);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(message);
        }
    }
}
