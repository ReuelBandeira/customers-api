using Api.Domain.UseCases.Customers.Models;

namespace Api.Domain.UseCases.Customers.Repositories.Interfaces;

public interface ICustomerRepository
{
    public IQueryable<Customer> GetAll();
    public Task<Customer?> GetByIdAsync(int id);
    public Task AddAsync(Customer customer);
    public Task UpdateAsync(Customer customer);
    public Task SoftDeleteAsync(Customer customer);
    public Task<bool> ValidateUpdateAsync(string name, int id = 0);
}