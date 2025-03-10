using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Customers.Services.Interfaces;

public interface ICustomerGetService
{
    public Task<PaginatedList<Customer>> GetAllAsync(PaginationParams paginationParams, CustomerFilterDto filterParams);
    public Task<Customer?> GetByIdAsync(int id);
}