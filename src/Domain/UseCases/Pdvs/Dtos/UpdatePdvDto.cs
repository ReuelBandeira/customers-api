using Api.Domain.UseCases.Pdvs.Dtos;

namespace Api.Domain.UseCases.Pdvs.Dtos;

public class UpdatePdvDto
{
    public int CustomerId { get; set; }
    public required int Days { get; set; }
    public required bool Status { get; set; }
}