using Api.Domain.UseCases.Customers.Dtos;

namespace Api.Domain.UseCases.Customers.Dtos;

public class CustomerFilterDto
{
    public string? Name { get; set; }
    public string? Cnpj { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? Key { get; set; }
}