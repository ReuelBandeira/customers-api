using Api.Domain.UseCases.Pdvs.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Infra.Database;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Pdvs.Repositories;

[ScopedService]
public class PdvRepository(AppDbContext dbContext) : IPdvRepository
{
    private readonly AppDbContext _context = dbContext;

    public IQueryable<Pdv> GetAll()
    {
        return _context.Pdv
                       .Include(x => x.Customer)
                       .OrderByDescending(x => x.CreatedAt)
                       .AsQueryable();
    }

    public async Task<Pdv?> GetByIdAsync(int id)
    {
        return await _context.Pdv
                             .Include(x => x.Customer)
                             .FirstOrDefaultAsync(x => x.PvdId == id);
    }

    public async Task AddAsync(Pdv pdv)
    {
        pdv.CreatedAt = DateTime.UtcNow;
        await _context.Pdv.AddAsync(pdv);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pdv pdv)
    {
        pdv.UpdatedAt = DateTime.UtcNow;
        _context.Pdv.Update(pdv);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Pdv pdv)
    {
        pdv.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(pdv);
    }

    // public async Task<bool> ValidateUpdateAsync(string name, int id = 0)
    // {
    //     return await _context.Pdv
    //                          .AnyAsync(x => x.CustomerId == name
    //                                      && x.PvdId != id);
    // }
}