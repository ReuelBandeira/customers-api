using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Customers.Services.Interfaces;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Customers.Services;

[ScopedService]
public class CustomerCreateService(ICustomerRepository repository) : ICustomerCreateService
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<Customer> AddAsync(CreateCustomerDto dto)
    {
        bool isNameTaken = await _repository.ValidateUpdateAsync(dto.Cnpj);

        if (isNameTaken)
        {
            throw new EntityNotCreatedException("Customer Cnpj already exists.");
        }

        var customer = new Customer
        {
            Name = dto.Name,
            Cnpj = dto.Cnpj,
            User = dto.User,
            Password = dto.Password,
            Key = dto.Key
        };

        await _repository.AddAsync(customer);

        return customer;
    }
}