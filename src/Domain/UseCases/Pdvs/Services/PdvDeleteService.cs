using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Domain.UseCases.Pdvs.Repositories;
using Api.Domain.UseCases.Pdvs.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Services.Interfaces;
using Api.Shared.Bases.Attributes;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Services;

[ScopedService]
public class NotificationDeleteService(IPdvRepository repository) : IPdvDeleteService
{
    private readonly IPdvRepository _repository = repository;

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var notification = await _repository.GetByIdAsync(id);

        if (notification == null) return false;

        await _repository.SoftDeleteAsync(notification);

        return true;
    }
}