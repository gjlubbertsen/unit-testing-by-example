﻿using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Contoso.Pizza.Data.Contracts;
using DM = Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.AdminApi.Services;

public class PizzaService : IPizzaService
{
    private readonly IPizzaValidator _pizzaValidator;
    private readonly IPizzaRepository _repository;
    private readonly IMapper _mapper;

    public PizzaService(IPizzaValidator pizzaValidator, IPizzaRepository repository, IMapper mapper)
    {
        _pizzaValidator = pizzaValidator;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PizzaEntity>> GetAllAsync()
    {
        var pizzas = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PizzaEntity>>(pizzas);
    }

    public async Task<PizzaEntity?> GetByIdAsync(Guid id)
    {
        var pizza = await _repository.GetByIdAsync(id);
        return _mapper.Map<PizzaEntity>(pizza);
    }

    public async Task<PizzaEntity> AddAsync(PizzaEntity entity)
    {
       var validationErrors = await _pizzaValidator.IsValidPizza(entity);
        if (validationErrors.Count != 0)
        {
            throw new ArgumentException(string.Join("; ", validationErrors));
        }
        var newPizza = _mapper.Map<DM.Pizza>(entity);
        await _repository.AddAsync(newPizza);
        return _mapper.Map<PizzaEntity>(newPizza);
    }

    public async Task<int> UpdateAsync(PizzaEntity entity)
    {
        var validationErrors = await _pizzaValidator.IsValidPizza(entity);
        if (validationErrors.Count != 0)
        {
            throw new ArgumentException(string.Join("; ", validationErrors));
        }
        var pizzaToUpdate = _mapper.Map<DM.Pizza>(entity);
        return await _repository.UpdateAsync(pizzaToUpdate);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
