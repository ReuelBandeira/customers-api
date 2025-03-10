using Api.Domain.UseCases.Pdvs.Dtos;

namespace Api.Domain.UseCases.Pdvs.Dtos;

public class CreatePdvDto
{
    public int CustomerId { get; set; }
    public int Days { get; set; }
    public required bool Status { get; set; }
}