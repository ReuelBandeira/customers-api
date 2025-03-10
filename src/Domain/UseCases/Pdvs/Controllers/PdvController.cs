using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Services.Interfaces;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Pdvs.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PdvController(IPdvGetService getService,
                              IPdvCreateService createService,
                              IPdvUpdateService updateService,
                              IPdvDeleteService deleteService) : ControllerBase
{
    private readonly IPdvCreateService _createService = createService;
    private readonly IPdvDeleteService _deleteService = deleteService;
    private readonly IPdvGetService _getService = getService;
    private readonly IPdvUpdateService _updateService = updateService;

    [HttpGet]
    [ServiceFilter(typeof(PaginationHeaderFilter))]
    public async Task<IActionResult> GetAll([FromServices] PaginationParams paginationParams, [FromQuery] PdvFilterDto filterParams)
    {
        var pdv = await _getService.GetAllAsync(paginationParams, filterParams);
        pdv.AddPaginationHeaders(Response);
        return Ok(pdv);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pdv = await _getService.GetByIdAsync(id);
        return pdv == null ? NotFound(new { message = "Pdv not exists." }) : Ok(pdv);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePdvDto dto)
    {
        try
        {
            var pdv = await _createService.AddAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = pdv.PvdId }, pdv);
        }
        catch (EntityNotCreatedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePdvDto dto)
    {
        try
        {
            var pdv = await _updateService.UpdateAsync(id, dto);
            return pdv == null ? NotFound(new { message = "Pdv not exists." }) : Ok(pdv);
        }
        catch (EntityNotUpdatedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _deleteService.SoftDeleteAsync(id) ? NoContent() : NotFound(new { message = "Pdv not exists." });
    }
}