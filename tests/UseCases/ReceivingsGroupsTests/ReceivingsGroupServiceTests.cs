using Api.Domain.UseCases.ReceivingsGroups.Dtos;
using Api.Domain.UseCases.ReceivingsGroups.Models;
using Api.Domain.UseCases.ReceivingsGroups.Repositories.Interfaces;
using Api.Domain.UseCases.ReceivingsGroups.Services;
using Api.Domain.UseCases.ReceivingsGroups.Dtos;
using Api.Domain.UseCases.ReceivingsPerson.Models;
using Api.Domain.UseCases.ReceivingsPerson.Dtos;

namespace Tests.UseCases.ReceivingsGroupsTests;
public class ReceivingsGroupserviceTests
{
    private Mock<IReceivingsGroupRepository> _repositoryMock;
    private ReceivingsGroupGetService _getService;
    private ReceivingsGroupCreateService _createService;
    private ReceivingsGroupUpdateService _updateService;
    private ReceivingsGroupDeleteService _deleteService;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IReceivingsGroupRepository>();
        _getService = new ReceivingsGroupGetService(_repositoryMock.Object);
        _createService = new ReceivingsGroupCreateService(
            _repositoryMock.Object
            );
        _updateService = new ReceivingsGroupUpdateService(_repositoryMock.Object);
        _deleteService = new ReceivingsGroupDeleteService(_repositoryMock.Object);

    }

    // Teste para GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectReceivingsGroup()
    {
        // Arrange
        var neceivingsGroupId = 1;
        var mockData = new ReceivingsGroup
        {
            RecGroId = neceivingsGroupId,
            RecGroName = "Nome do Grupo",
            RecGroStatus = true
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(neceivingsGroupId)).ReturnsAsync(mockData);

        // Act
        var result = await _getService.GetByIdAsync(neceivingsGroupId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(neceivingsGroupId, Is.EqualTo(result?.RecGroId));
        _repositoryMock.Verify(r => r.GetByIdAsync(neceivingsGroupId), Times.Once);
    }

    // Teste para AddAsync
    [Test]
    public async Task AddAsync_ShouldReturnAddedReceivingsGroup()
    {
        // Arrange
        var dto = new CreateReceivingsGroupDto
        {
            RecGroName = "Nome do Grupo",
            RecGroStatus = true,
            PersonList = new List<CreateReceivingPersonDto>
        {
            new CreateReceivingPersonDto { RecPerPvdId = 1 }
        }
        };

        _repositoryMock.Setup(r => r.ValidateNameGroupAsync(dto.RecGroName, 0))
            .ReturnsAsync(false);

        var receivingsGroup = new ReceivingsGroup
        {
            RecGroName = dto.RecGroName,
            RecGroStatus = dto.RecGroStatus,
            ReceivingPersonList = new List<ReceivingPerson>
            {
                new ReceivingPerson { RecPerPvdId = 1 }
            }
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<ReceivingsGroup>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _createService.AddAsync(dto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsNotNull(result);
            Assert.That(result.RecGroName, Is.EqualTo(dto.RecGroName));
            Assert.That(result.RecGroStatus, Is.EqualTo(dto.RecGroStatus));
            Assert.That(result.ReceivingPersonList, Is.Not.Null);
            Assert.That(result.ReceivingPersonList.Count, Is.EqualTo(1)); // ✅ Garantir que a lista tem 1 item
        });

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ReceivingsGroup>()), Times.Once);
    }



    // Teste para UpdateAsync
    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedReceivingsGroup()
    {
        // Arrange
        var receivingsGroupId = 1;
        var dto = new UpdateReceivingsGroupDto
        {
            RecGroName = "Novo Nome do Grupo",
            RecGroStatus = false,
            // Adicionando a lista de pessoas de recebimento
            PersonList = new List<UpdateReceivingPersonDto>
            {
                new UpdateReceivingPersonDto
                {
                    RecPvdId = 1,
                    RecPerPvdId = 1, // Defina os campos necessários
                    RecPerRecGroId = receivingsGroupId // Relacionando ao grupo de recebimento
                }
            }
        };

        var existingLine = new ReceivingsGroup
        {
            RecGroName = "Nome Antigo do Grupo",
            RecGroStatus = true,
            ReceivingPersonList = new List<ReceivingPerson>
            {
                new ReceivingPerson { RecPerPvdId = 1 } // Simulando que o "ReceivingPerson" existe
            }
        };

        _repositoryMock.Setup(r => r.ValidateUpdateAsync(dto.RecGroName, receivingsGroupId))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.GetByIdAsync(receivingsGroupId))
            .ReturnsAsync(existingLine);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ReceivingsGroup>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ReceivingsGroup>())).Returns(Task.CompletedTask);

        // Act
        var result = await _updateService.UpdateAsync(receivingsGroupId, dto);

        Assert.Multiple(() =>
        {
            Assert.IsNotNull(result);
            Assert.That(result.RecGroId, Is.EqualTo(receivingsGroupId));
            Assert.That(result.RecGroName, Is.EqualTo(dto.RecGroName));
            Assert.That(result.RecGroStatus, Is.EqualTo(dto.RecGroStatus));
        });

        Assert.That(dto.RecGroName, Is.EqualTo(result?.RecGroName));
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ReceivingsGroup>()), Times.Once);
    }


    // Teste para DeleteAsync
    [Test]
    public async Task DeleteAsync_ShouldReturnTrueWhenDeleted()
    {
        // Arrange
        var receivingsGroupId = 1;
        var existingLine = new ReceivingsGroup
        {
            RecGroId = receivingsGroupId,
            RecGroName = "Nome do Grupo",
            RecGroStatus = true
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(receivingsGroupId)).ReturnsAsync(existingLine);
        _repositoryMock.Setup(r => r.SoftDeleteAsync(It.IsAny<ReceivingsGroup>())).Returns(Task.CompletedTask);

        // Act
        var result = await _deleteService.SoftDeleteAsync(receivingsGroupId);

        // Assert
        Assert.IsTrue(result);
        _repositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<ReceivingsGroup>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var ReceivingsGroupId = 1;
        _repositoryMock.Setup(r => r.GetByIdAsync(ReceivingsGroupId)).ReturnsAsync(default(ReceivingsGroup));

        // Act
        var result = await _deleteService.SoftDeleteAsync(ReceivingsGroupId);

        // Assert
        Assert.IsFalse(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(ReceivingsGroupId), Times.Once);
    }
}