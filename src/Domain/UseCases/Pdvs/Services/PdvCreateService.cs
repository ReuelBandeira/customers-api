using Api.Domain.UseCases.Customers.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Dtos;
using Api.Domain.UseCases.Pdvs.Models;
using Api.Domain.UseCases.Pdvs.Repositories.Interfaces;
using Api.Domain.UseCases.Pdvs.Services.Interfaces;
using Api.Shared.Bases.Attributes;

namespace Api.Domain.UseCases.Pdvs.Services;

[ScopedService]
public class PdvCreateService(IPdvRepository repository, ICustomerRepository customerRepository) : IPdvCreateService
{
    private readonly IPdvRepository _repository = repository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<Pdv> AddAsync(CreatePdvDto dto)
    {
        // bool isNameTaken = await _repository.ValidateUpdateAsync(dto.CustomerId);

        // if (isNameTaken)
        // {
        //     throw new EntityNotCreatedException("Pdv name already exists.");
        // }

        var customerValidation = await _customerRepository.GetByIdAsync(dto.CustomerId);

        if (customerValidation == null)
        {
            throw new EntityNotCreatedException("notification not found.");
        }

        var pdv = new Pdv
        {
            CustomerId = dto.CustomerId,
            Days = dto.Days,
            Status = dto.Status
        };

        await _repository.AddAsync(pdv);

        return pdv;
    }
}