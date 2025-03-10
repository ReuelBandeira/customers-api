using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Models;
using Api.Domain.UseCases.Customers.Repositories;
using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Customers.Services.Interfaces;
using Api.Shared.Bases.Attributes;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Customers.Services;

[ScopedService]
public class CustomerGetService(ICustomerRepository repository) : ICustomerGetService
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<PaginatedList<Customer>> GetAllAsync(PaginationParams paginationParams, CustomerFilterDto filterParams)
    {
        var query = _repository.GetAll();

        // Aplicar filtros
        if (!string.IsNullOrEmpty(filterParams.Cnpj))
        {
            query = query.Where(x => x.Cnpj.Contains(filterParams.Cnpj));
        }

        if (!string.IsNullOrEmpty(filterParams.User)) // Fixed null check
        {
            query = query.Where(x => x.User.Contains(filterParams.User)); // Fixed comparison
        }

        // Retornar resultados paginados
        return await PaginatedList<Customer>.CreateAsync(query, paginationParams.pageNumber, paginationParams.pageSize);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}