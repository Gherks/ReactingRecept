using ReactingRecept.Domain.Base;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface IAsyncRepository<Type> where Type : BaseEntity
{
    Task<bool> AnyAsync(Guid id);
    Task<Type?> GetByIdAsync(Guid id);
    Task<Type[]?> GetAllAsync();
    Task<Type?> AddAsync(Type entity);
    Task<Type[]?> AddManyAsync(Type[] entities);
    Task<Type?> UpdateAsync(Type entity);
    Task<Type[]?> UpdateManyAsync(Type[] entities);
    Task<bool> DeleteAsync(Type entity);
    Task<bool> DeleteManyAsync(Type[] entities);
}
