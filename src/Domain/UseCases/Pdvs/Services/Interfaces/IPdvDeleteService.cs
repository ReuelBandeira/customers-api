using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Services.Interfaces;

public interface IPdvDeleteService
{
    public Task<bool> SoftDeleteAsync(int id);
}