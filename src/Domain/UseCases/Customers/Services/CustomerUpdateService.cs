using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Customers.Services.Interfaces;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Notifications.Services;

[ScopedService]
public class CustomerUpdateService(ICustomerRepository repository) : ICustomerUpdateService
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<Customer?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _repository.GetByIdAsync(id);

        if (customer is null)
        {
            throw new EntityNotUpdatedException("Customer not exists.");
        }

        bool isNameTaken = await _repository.ValidateUpdateAsync(dto.Cnpj, id);

        if (isNameTaken)
        {
            throw new EntityNotUpdatedException("Customer name already exists.");
        }

        customer.CustomerId = id;
        customer.Name = dto.Name;
        customer.Cnpj = dto.Cnpj;
        customer.User = dto.User;
        customer.Password = dto.Password;
        customer.Key = dto.Key;

        await _repository.UpdateAsync(customer);

        return customer;
    }
}