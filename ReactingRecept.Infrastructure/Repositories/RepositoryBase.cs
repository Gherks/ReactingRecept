using Microsoft.EntityFrameworkCore;
using ReactingRecept.Server.Entities.Bases;
using ReactingRecept.Infrastructure.Context;
using ReactingRecept.Infrastructure.Repositories.Interfaces;

namespace ReactingRecept.Infrastructure.Repositories;

public class RepositoryBase<Type> : IAsyncRepository<Type> where Type : DomainEntityBase
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

    public virtual async Task<IReadOnlyList<Type>?> ListAllAsync()
    {
        try
        {
            return await _reactingReceptContext.Set<Type>().ToListAsync();
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

    public async Task<IEnumerable<Type>?> AddManyAsync(IEnumerable<Type> entities)
    {
        try
        {
            await _reactingReceptContext.Set<Type>().AddRangeAsync(entities);
            await _reactingReceptContext.SaveChangesAsync();

            foreach (var entity in entities)
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

    public virtual async Task<Type> UpdateAsync(Type entity)
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

    public virtual async Task UpdateManyAsync(IEnumerable<Type> entities)
    {
        try
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (Type entity in entities)
            {
                _reactingReceptContext.Entry(entity).State = EntityState.Modified;
            }

            await _reactingReceptContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            //Log.Error(exception, $"Repository failed to update many entities: {entities}");
        }
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

    public virtual async Task<bool> DeleteManyAsync(IEnumerable<Type> entities)
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

    protected internal void ApplyAddedOrModifiedState(DomainEntityBase domainEntityBase)
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
