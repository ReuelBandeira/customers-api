using Api.Domain.UseCases.Pdvs.Models;

namespace Api.Domain.UseCases.Pdvs.Repositories.Interfaces;

public interface IPdvRepository
{
    public IQueryable<Pdv> GetAll();
    public Task<Pdv?> GetByIdAsync(int id);
    public Task AddAsync(Pdv pdv);
    public Task UpdateAsync(Pdv pdv);
    public Task SoftDeleteAsync(Pdv pdv);
    // public Task<bool> ValidateUpdateAsync(string name, int id = 0);
}