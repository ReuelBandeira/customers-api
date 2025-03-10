using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Services.Interfaces;

public interface IPdvGetService
{
    public Task<PaginatedList<Pdv>> GetAllAsync(PaginationParams paginationParams, PdvFilterDto filterParams);
    public Task<Pdv?> GetByIdAsync(int id);
}