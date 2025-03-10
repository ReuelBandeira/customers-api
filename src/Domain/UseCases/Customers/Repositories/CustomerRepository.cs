using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Customers.Models;
using Api.Infra.Database;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Customers.Repositories;

[ScopedService]
public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    private readonly AppDbContext _context = dbContext;

    public IQueryable<Customer> GetAll()
    {
        return _context.Customer
                       .OrderByDescending(x => x.CreatedAt)
                       .AsQueryable();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customer
                             .FirstOrDefaultAsync(x => x.CustomerId == id);
    }

    public async Task AddAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        await _context.Customer.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customer.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Customer customer)
    {
        customer.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(customer);
    }

    public async Task<bool> ValidateUpdateAsync(string name, int id = 0)
    {
        return await _context.Customer
                             .AnyAsync(x => x.Cnpj == name
                                         && x.CustomerId != id);
    }
}