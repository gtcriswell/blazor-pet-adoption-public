using API.Business;
using API.Controllers;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace apitests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task AddVisitorAsync_ReturnsBadRequest_WhenEmailIsEmpty()
        {
            var userBusiness = new Mock<IUserBusiness>();
            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddVisitorAsync(email: "");

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email is required.", badRequest.Value);
        }

        [Fact]
        public async Task AddVisitorAsync_ReturnsOkWithVisitor_WhenServiceReturnsVisitor()
        {
            var visitor = new Visitor { Email = "test@example.com", CreatedDate = DateTime.UtcNow };
            var userBusiness = new Mock<IUserBusiness>();
            userBusiness.Setup(s => s.AddVistorAsync(It.IsAny<Visitor>()))
                       .ReturnsAsync(visitor);

            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddVisitorAsync(email: "test@example.com");

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Visitor>(ok.Value);
            Assert.Equal("test@example.com", returned.Email);
        }

        [Fact]
        public async Task AddVisitorAsync_ReturnsOkWithEmptyVisitor_WhenServiceReturnsNull()
        {
            var userBusiness = new Mock<IUserBusiness>();
            userBusiness.Setup(s => s.AddVistorAsync(It.IsAny<Visitor>()))
                       .ReturnsAsync((Visitor?)null);

            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddVisitorAsync(email: "noreturn@example.com");

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Visitor>(ok.Value);
            Assert.NotNull(returned);
        }

        [Fact]
        public async Task AddVisitorAsync_ReturnsProblemOnException()
        {
            var userBusiness = new Mock<IUserBusiness>();
            userBusiness.Setup(s => s.AddVistorAsync(It.IsAny<Visitor>()))
                       .ThrowsAsync(new Exception("boom"));

            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddVisitorAsync(email: "err@example.com");

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddTracker_ReturnsOkWithTracker_WhenServiceReturnsTracker()
        {
            var tracker = new Tracker { /* set properties if needed */ };
            var userBusiness = new Mock<IUserBusiness>();
            userBusiness.Setup(s => s.AddTrackerAsync(It.IsAny<Tracker>()))
                       .ReturnsAsync(tracker);

            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddTracker(tracker);

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Tracker>(ok.Value);
            Assert.Same(tracker, returned);
        }

        [Fact]
        public async Task AddTracker_ReturnsOkWithVisitor_WhenServiceReturnsNull()
        {
            var userBusiness = new Mock<IUserBusiness>();
            userBusiness.Setup(s => s.AddTrackerAsync(It.IsAny<Tracker>()))
                       .ReturnsAsync((Tracker?)null);

            var logger = new Mock<ILogger<UserController>>();
            var controller = new UserController(userBusiness.Object, logger.Object);

            var result = await controller.AddTracker(new Tracker());

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Visitor>(ok.Value);
            Assert.NotNull(returned);
        }
    }
}