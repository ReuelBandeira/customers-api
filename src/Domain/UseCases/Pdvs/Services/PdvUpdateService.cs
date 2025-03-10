using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Domain.UseCases.Pdvs.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Services.Interfaces;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Notifications.Services;

[ScopedService]
public class PdvUpdateService(IPdvRepository repository, ICustomerRepository customerRepository) : IPdvUpdateService
{
    private readonly IPdvRepository _repository = repository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<Pdv?> UpdateAsync(int id, UpdatePdvDto dto)
    {
        var pdv = await _repository.GetByIdAsync(id);

        if (pdv is null)
        {
            throw new EntityNotUpdatedException("Pdv not exists.");
        }

        // bool isNameTaken = await _repository.ValidateUpdateAsync(dto.CustomerId, id);

        // if (isNameTaken)
        // {
        //     throw new EntityNotUpdatedException("Pdv name already exists.");
        // }

        var customerValidation = await _customerRepository.GetByIdAsync(dto.CustomerId);

        if (customerValidation == null)
        {
            throw new EntityNotUpdatedException("notification not found.");
        }

        pdv.PvdId = id;
        pdv.CustomerId = dto.CustomerId;
        pdv.Days = dto.Days;
        pdv.Status = dto.Status;

        await _repository.UpdateAsync(pdv);

        return pdv;
    }
}