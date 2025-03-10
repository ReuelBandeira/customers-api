using Api.Domain.UseCases.Notifications.Dtos;
using Api.Domain.UseCases.Notifications.Models;
using Api.Domain.UseCases.Notifications.Repositories.Interfaces;
using Api.Domain.UseCases.Notifications.Services;
using Api.Domain.UseCases.NotificationsGroup.Dtos;
using Api.Domain.UseCases.NotificationsGroup.DTOs;
using Api.Domain.UseCases.NotificationsGroup.Models;

namespace Tests.UseCases.NotificationsTests;
public class NotificationServiceTests
{
    private Mock<INotificationRepository> _repositoryMock;
    private NotificationGetService _getService;
    private NotificationCreateService _createService;
    private NotificationUpdateService _updateService;
    private NotificationDeleteService _deleteService;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<INotificationRepository>();
        _getService = new NotificationGetService(_repositoryMock.Object);
        _createService = new NotificationCreateService(_repositoryMock.Object);
        _updateService = new NotificationUpdateService(_repositoryMock.Object);
        _deleteService = new NotificationDeleteService(_repositoryMock.Object);
    }

    // Teste para GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectNotification()
    {
        // Arrange
        var notificationId = 1;
        var mockData = new Notification
        {
            NotId = notificationId,
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(notificationId)).ReturnsAsync(mockData);

        // Act
        var result = await _getService.GetByIdAsync(notificationId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(notificationId, Is.EqualTo(result?.NotId));
        _repositoryMock.Verify(r => r.GetByIdAsync(notificationId), Times.Once);
    }

    // Teste para AddAsync
    [Test]
    public async Task AddAsync_ShouldReturnAddedNotification()
    {
        // Arrange
        var groupsDto = new List<CreateNotificationGroupDto>
        {
            new CreateNotificationGroupDto
            {
                NotGroRecGroId = 1
            }
        };

        var dto = new CreateNotificationDto
        {
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true,
            ReceivingsGroupList = groupsDto
        };

        // Configurando o mock com o id padrão 0
        _repositoryMock.Setup(r => r.ValidateUpdateAsync(dto.NotName, 0))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _createService.AddAsync(dto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.NotName, Is.EqualTo(dto.NotName));
            Assert.That(result.NotText, Is.EqualTo(dto.NotText));
            Assert.That(result.NotStatus, Is.EqualTo(dto.NotStatus));
            Assert.That(result.NotificationGroupList, Is.Not.Null);
        });

        _repositoryMock.Verify(r => r.ValidateUpdateAsync(dto.NotName, 0), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Notification>(n =>
            n.NotName == dto.NotName &&
            n.NotText == dto.NotText &&
            n.NotStatus == dto.NotStatus)), Times.Once);
    }

    // Teste para UpdateAsync
    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedNotification()
    {
        // Arrange
        var notificationId = 1;
        var groupsDto = new List<UpdateNotificationGroupDto>
        {
            new UpdateNotificationGroupDto
            {
                NotGroRecGroId = 1
            }
        };

        var dto = new UpdateNotificationDto
        {
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true,
            ReceivingsGroupList = groupsDto
        };

        var existingNotification = new Notification
        {
            NotId = notificationId,
            NotName = "Título Antigo",
            NotText = "Mensagem Antiga",
            NotStatus = false,
            NotificationGroupList = new List<NotificationGroup>() // Lista inicial vazia ou com grupos existentes
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(notificationId))
            .ReturnsAsync(existingNotification);

        _repositoryMock.Setup(r => r.ValidateUpdateAsync(dto.NotName, notificationId))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Notification>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _updateService.UpdateAsync(notificationId, dto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.NotId, Is.EqualTo(notificationId));
            Assert.That(result.NotName, Is.EqualTo(dto.NotName));
            Assert.That(result.NotText, Is.EqualTo(dto.NotText));
            Assert.That(result.NotStatus, Is.EqualTo(dto.NotStatus));
            Assert.That(result.NotificationGroupList, Is.Not.Null);
        });

        _repositoryMock.Verify(r => r.GetByIdAsync(notificationId), Times.Once);
        _repositoryMock.Verify(r => r.ValidateUpdateAsync(dto.NotName, notificationId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Notification>(n =>
            n.NotId == notificationId &&
            n.NotName == dto.NotName &&
            n.NotText == dto.NotText &&
            n.NotStatus == dto.NotStatus)), Times.Once);
    }

    // Teste para DeleteAsync
    [Test]
    public async Task DeleteAsync_ShouldReturnTrueWhenDeleted()
    {
        // Arrange
        var notificationId = 1;
        var existingLine = new Notification
        {
            NotId = notificationId,
            NotName = "Título",
            NotText = "Mensagem",
            NotStatus = true
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(notificationId)).ReturnsAsync(existingLine);
        _repositoryMock.Setup(r => r.SoftDeleteAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);

        // Act
        var result = await _deleteService.SoftDeleteAsync(notificationId);

        // Assert
        Assert.IsTrue(result);
        _repositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Notification>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var notificationId = 1;
        _repositoryMock.Setup(r => r.GetByIdAsync(notificationId)).ReturnsAsync(default(Notification));

        // Act
        var result = await _deleteService.SoftDeleteAsync(notificationId);

        // Assert
        Assert.IsFalse(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(notificationId), Times.Once);
    }
}