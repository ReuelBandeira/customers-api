using Api.Domain.UseCases.Customers.Dtos;
using Api.Domain.UseCases.Customers.Services.Interfaces;
using Api.Shared.Helpers;

namespace Api.Domain.UseCases.Customers.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomerController(ICustomerGetService getService,
                              ICustomerCreateService createService,
                              ICustomerUpdateService updateService,
                              ICustomerDeleteService deleteService) : ControllerBase
{
    private readonly ICustomerCreateService _createService = createService;
    private readonly ICustomerDeleteService _deleteService = deleteService;
    private readonly ICustomerGetService _getService = getService;
    private readonly ICustomerUpdateService _updateService = updateService;

    [HttpGet]
    [ServiceFilter(typeof(PaginationHeaderFilter))]
    public async Task<IActionResult> GetAll([FromServices] PaginationParams paginationParams, [FromQuery] CustomerFilterDto filterParams)
    {
        var customer = await _getService.GetAllAsync(paginationParams, filterParams);
        customer.AddPaginationHeaders(Response);
        return Ok(customer);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _getService.GetByIdAsync(id);
        return customer == null ? NotFound(new { message = "Customer not exists." }) : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        try
        {
            var customer = await _createService.AddAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
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
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        try
        {
            var customer = await _updateService.UpdateAsync(id, dto);
            return customer == null ? NotFound(new { message = "Customer not exists." }) : Ok(customer);
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
        return await _deleteService.SoftDeleteAsync(id) ? NoContent() : NotFound(new { message = "Customer not exists." });
    }
}