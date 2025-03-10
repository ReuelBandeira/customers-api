using Api.Domain.UseCases.ReceivingsGroups.Controllers;
using Api.Domain.UseCases.ReceivingsGroups.Dtos;
using Api.Domain.UseCases.ReceivingsGroups.Models;
using Api.Domain.UseCases.ReceivingsGroups.Services;
using Api.Domain.UseCases.ReceivingsGroups.Services.Interfaces;

namespace Tests.UseCases.ReceivingsGroupsTests;

[TestFixture]
public class ReceivingsGroupControllerTests
{
    private Mock<IReceivingsGroupGetService> _getServiceMock;
    private Mock<IReceivingsGroupCreateService> _createServiceMock;
    private Mock<IReceivingsGroupUpdateService> _updateServiceMock;
    private Mock<IReceivingsGroupDeleteService> _deleteServiceMock;

    private ReceivingsGroupController _controller;

    [SetUp]
    public void SetUp()
    {
        _getServiceMock = new Mock<IReceivingsGroupGetService>();
        _createServiceMock = new Mock<IReceivingsGroupCreateService>();
        _updateServiceMock = new Mock<IReceivingsGroupUpdateService>();
        _deleteServiceMock = new Mock<IReceivingsGroupDeleteService>();

        _controller = new ReceivingsGroupController(
            _getServiceMock.Object,
            _createServiceMock.Object,
            _updateServiceMock.Object,
            _deleteServiceMock.Object
        );
    }

    [Test]
    public async Task GetById_ReturnsOkResult_WhenReceivingsGroupExists()
    {
        // Arrange
        var receivingsGroup = new ReceivingsGroup
        {
            RecGroId = 1,
            RecGroName = "Nome do Grupo",
            RecGroStatus = true,
            CreatedAt = DateTime.UtcNow
        };
        _getServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(receivingsGroup);

        // Act
        var result = await _controller.GetById(1) as OkObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(result.Value, Is.EqualTo(receivingsGroup));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenReceivingsGroupDoesNotExist()
    {
        // Arrange
        _getServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(default(ReceivingsGroup));

        // Act
        var result = await _controller.GetById(1) as NotFoundObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        Assert.That(result.Value?.GetType().GetProperty("message")?.GetValue(result.Value), Is.EqualTo("Receivings Group not exists."));
    }

    [Test]
    public async Task Create_ReturnsCreatedResult_WhenReceivingsGroupIsCreated()
    {
        // Arrange
        var createDto = new CreateReceivingsGroupDto
        {
            RecGroName = "Nome do Grupo",
            RecGroStatus = true
        };
        var receivingsGroup = new ReceivingsGroup
        {
            RecGroName = "Nome do Grupo",
            RecGroStatus = true
        };

        _createServiceMock.Setup(service => service.AddAsync(createDto)).ReturnsAsync(receivingsGroup);

        // Act
        var result = await _controller.Create(createDto) as CreatedAtActionResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetById)));
        Assert.That(result.Value, Is.EqualTo(receivingsGroup));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenEntityNotCreatedExceptionIsThrown()
    {
        // Arrange
        var createDto = new CreateReceivingsGroupDto
        {
            RecGroName = "Nome do Grupo",
            RecGroStatus = true
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
    public async Task Delete_ReturnsNoContent_WhenReceivingsGroupIsDeleted()
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
    public async Task Delete_ReturnsNotFound_WhenReceivingsGroupDoesNotExist()
    {
        // Arrange
        _deleteServiceMock.Setup(service => service.SoftDeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1) as NotFoundObjectResult ?? throw new InvalidOperationException("Result is null");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        Assert.That(result.Value?.GetType().GetProperty("message")?.GetValue(result.Value), Is.EqualTo("Receivings Group not exists."));
    }
}