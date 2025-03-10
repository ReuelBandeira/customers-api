using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Services.Interfaces;

public interface IPdvUpdateService
{
    public Task<Pdv?> UpdateAsync(int id, UpdatePdvDto dto);
}