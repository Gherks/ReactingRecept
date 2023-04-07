﻿using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<bool> AnyAsync(Guid id);
        Task<IngredientDTO?> GetByIdAsync(Guid id);
        Task<IngredientDTO[]?> GetAllAsync();
        //Task<IIngredientDto[]?> GetAllAsync();
        //Task<IIngredientDto?> AddAsync(IIngredientDto ingredient);
        //Task<IIngredientDto[]?> AddManyAsync(IIngredientDto[] ingredients);
        //Task<IIngredientDto?> UpdateAsync(IIngredientDto ingredient);
        //Task<IIngredientDto[]?> UpdateManyAsync(IIngredientDto[] ingredients);
        //Task<bool> DeleteAsync(IIngredientDto ingredient);
        //Task<bool> DeleteManyAsync(IIngredientDto[] ingredients);
    }
}
