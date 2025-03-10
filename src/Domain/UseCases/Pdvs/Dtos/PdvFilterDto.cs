using Api.Domain.UseCases.Pdvs.Dtos;

namespace Api.Domain.UseCases.Pdvs.Dtos;

public class PdvFilterDto
{
    public int? CustomerId { get; set; }
    public int? Days { get; set; }
    public bool? Status { get; set; }
}