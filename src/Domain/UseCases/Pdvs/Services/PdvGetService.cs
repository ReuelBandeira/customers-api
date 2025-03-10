using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Domain.UseCases.Pdvs.Repositories;
using Api.Domain.UseCases.Pdvs.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Services.Interfaces;
using Api.Shared.Bases.Attributes;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Services;

[ScopedService]
public class PdvGetService(IPdvRepository repository) : IPdvGetService
{
    private readonly IPdvRepository _repository = repository;

    public async Task<PaginatedList<Pdv>> GetAllAsync(PaginationParams paginationParams, PdvFilterDto filterParams)
    {
        var query = _repository.GetAll();

        // Aplicar filtros
        // if (!string.IsNullOrEmpty(filterParams.CustomerId))
        // {
        //     query = query.Where(x => x.CustomerId.Contains(filterParams.CustomerId));
        // }

        // if (!string.IsNullOrEmpty(filterParams.Days)) // Fixed null check
        // {
        //     query = query.Where(x => x.Days.Contains(filterParams.Days)); // Fixed comparison
        // }

        // Retornar resultados paginados
        return await PaginatedList<Pdv>.CreateAsync(query, paginationParams.pageNumber, paginationParams.pageSize);
    }

    public async Task<Pdv?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}