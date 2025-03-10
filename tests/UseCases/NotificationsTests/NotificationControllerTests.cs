using Api.Domain.UseCases.Notifications.Controllers;
using Api.Domain.UseCases.Notifications.Dtos;
using Api.Domain.UseCases.Notifications.Models;
using Api.Domain.UseCases.Notifications.Services;
using Api.Domain.UseCases.Notifications.Services.Interfaces;

namespace Tests.UseCases.NotificationsTests;

[TestFixture]
public class NotificationControllerTests
{
    private Mock<INotificationGetService> _getServiceMock;
    private Mock<INotificationCreateService> _createServiceMock;
    private Mock<INotificationUpdateService> _updateServiceMock;
    private Mock<INotificationDeleteService> _deleteServiceMock;

    private NotificationController _controller;

    [SetUp]
    public void SetUp()
    {
        _getServiceMock = new Mock<INotificationGetService>();
        _createServiceMock = new Mock<INotificationCreateService>();
        _updateServiceMock = new Mock<INotificationUpdateService>();
        _deleteServiceMock = new Mock<INotificationDeleteService>();

        _controller = new NotificationController(
            _getServiceMock.Object,
            _createServiceMock.Object,
            _updateServiceMock.Object,
            _deleteServiceMock.Object
        );
    }

    [Test]
    public async Task GetById_ReturnsOkResult_WhenNotificationExists()
    {
        // Arrange
        var notification = new Notification
        {
            NotId = 1,
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true,
            CreatedAt = DateTime.UtcNow
        };
        _getServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(notification);

        // Act
        var result = await _controller.GetById(1) as OkObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(result.Value, Is.EqualTo(notification));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenNotificationDoesNotExist()
    {
        // Arrange
        _getServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(default(Notification));

        // Act
        var result = await _controller.GetById(1) as NotFoundObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        Assert.That(result.Value?.GetType().GetProperty("message")?.GetValue(result.Value), Is.EqualTo("Notification not exists."));
    }

    [Test]
    public async Task Create_ReturnsCreatedResult_WhenNotificationIsCreated()
    {
        // Arrange
        var createDto = new CreateNotificationDto
        {
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true
        };
        var notification = new Notification
        {
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true
        };

        _createServiceMock.Setup(service => service.AddAsync(createDto)).ReturnsAsync(notification);

        // Act
        var result = await _controller.Create(createDto) as CreatedAtActionResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetById)));
        Assert.That(result.Value, Is.EqualTo(notification));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenEntityNotCreatedExceptionIsThrown()
    {
        // Arrange
        var createDto = new CreateNotificationDto
        {
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true
        };

        _createServiceMock.Setup(service => service.AddAsync(createDto))
            .ThrowsAsync(new Api.Shared.Exceptions.EntityNotCreatedException("Error creating entity"));

        // Act
        var result = await _controller.Create(createDto) as BadRequestObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(result.Value?.GetType().GetProperty("message")?.GetValue(result.Value), Is.EqualTo("Error creating entity"));

    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenNotificationIsDeleted()
    {
        // Arrange
        _deleteServiceMock.Setup(service => service.SoftDeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1) as NoContentResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenNotificationDoesNotExist()
    {
        // Arrange
        _deleteServiceMock.Setup(service => service.SoftDeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1) as NotFoundObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        Assert.That(result.Value?.GetType().GetProperty("message")?.GetValue(result.Value), Is.EqualTo("Notification not exists."));

    }
}