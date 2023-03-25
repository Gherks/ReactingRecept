using Microsoft.EntityFrameworkCore;
using ReactingRecept.Domain.Base;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class RepositoryBase<Type> : IAsyncRepository<Type> where Type : BaseEntity
{
    protected internal ReactingReceptContext _reactingReceptContext;

    public RepositoryBase(ReactingReceptContext reactingReceptContext)
    {
        _reactingReceptContext = reactingReceptContext;
    }

    public virtual async Task<bool> AnyAsync(Guid id)
    {
        try
        {
            return await _reactingReceptContext.Set<Type>().AnyAsync(entity => entity.Id == id);
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to check existence of entity with id: {id}");
            return false;
        }
    }

    public virtual async Task<Type?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _reactingReceptContext.Set<Type>().FindAsync(id);
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to fetch entity with id: {id}");
            return null;
        }
    }

    public virtual async Task<Type[]?> GetAllAsync()
    {
        try
        {
            return await _reactingReceptContext.Set<Type>().ToArrayAsync();
        }
        catch (Exception)
        {
            //Log.Error(exception, "Repository failed to fetch many entities");
            return null;
        }
    }

    public virtual async Task<Type?> AddAsync(Type entity)
    {
        try
        {
            _reactingReceptContext.Set<Type>().Add(entity);

            await _reactingReceptContext.SaveChangesAsync();
            await _reactingReceptContext.Entry(entity).ReloadAsync();
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to add entity: {entity}");
            return null;
        }

        return entity;
    }

    public async Task<Type[]?> AddManyAsync(Type[] entities)
    {
        try
        {
            await _reactingReceptContext.Set<Type>().AddRangeAsync(entities);
            await _reactingReceptContext.SaveChangesAsync();

            foreach (Type entity in entities)
            {
                await _reactingReceptContext.Entry(entity).ReloadAsync();
            }
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to add many entities: {entities}");
            return null;
        }

        return entities;
    }

    public virtual async Task<Type?> UpdateAsync(Type entity)
    {
        try
        {
            _reactingReceptContext.Entry(entity).State = EntityState.Modified;

            await _reactingReceptContext.SaveChangesAsync();
            await _reactingReceptContext.Entry(entity).ReloadAsync();
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to update entity: {entity}");
        }

        return entity;
    }

    public virtual async Task<Type[]?> UpdateManyAsync(Type[] entities)
    {
        try
        {
            if (!entities.Any())
            {
                return Array.Empty<Type>();
            }

            foreach (Type entity in entities)
            {
                _reactingReceptContext.Entry(entity).State = EntityState.Modified;
            }

            await _reactingReceptContext.SaveChangesAsync();

            //for (int index = 0; index < entities.Length; ++index)
            //{
            //    await _reactingReceptContext.Entry(entities[index]).ReloadAsync();
            //}

            foreach (Type entity in entities)
            {
                await _reactingReceptContext.Entry(entity).ReloadAsync();
            }
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to update many entities: {entities}");

            return Array.Empty<Type>();
        }

        return entities;
    }

    public virtual async Task<bool> DeleteAsync(Type entity)
    {
        try
        {
            _reactingReceptContext.Set<Type>().Remove(entity);
            await _reactingReceptContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to delete entity: {entity}");
            return false;
        }
    }

    public virtual async Task<bool> DeleteManyAsync(Type[] entities)
    {
        try
        {
            _reactingReceptContext.Set<Type>().RemoveRange(entities);
            await _reactingReceptContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to delete many entities: {entities}");
            return false;
        }
    }

    protected internal void ApplyAddedOrModifiedState(BaseEntity domainEntityBase)
    {
        if (domainEntityBase.Id != Guid.Empty)
        {
            _reactingReceptContext.Entry(domainEntityBase).State = EntityState.Modified;
        }
        else
        {
            _reactingReceptContext.Entry(domainEntityBase).State = EntityState.Added;
        }
    }
}
