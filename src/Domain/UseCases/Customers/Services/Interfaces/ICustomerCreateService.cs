using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Customers.Services.Interfaces;

public interface ICustomerCreateService
{
    public Task<Customer> AddAsync(CreateCustomerDto dto);
}