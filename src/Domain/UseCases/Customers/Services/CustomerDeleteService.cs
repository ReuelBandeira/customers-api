using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Domain.UseCases.Customers.Repositories;
using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Customers.Services.Interfaces;
using Api.Shared.Bases.Attributes;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Customers.Services;

[ScopedService]
public class NotificationDeleteService(ICustomerRepository repository) : ICustomerDeleteService
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var notification = await _repository.GetByIdAsync(id);

        if (notification == null) return false;

        await _repository.SoftDeleteAsync(notification);

        return true;
    }
}