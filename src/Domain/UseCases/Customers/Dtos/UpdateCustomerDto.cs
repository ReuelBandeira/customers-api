using Api.Domain.UseCases.Customers.Dtos;

namespace Api.Domain.UseCases.Customers.Dtos;

public class UpdateCustomerDto
{
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
    public required string Key { get; set; }
}