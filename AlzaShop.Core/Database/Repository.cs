using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace AlzaShop.Core.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected DbSet<TEntity> DbSet;
    protected readonly AlzaShopDbContext dbContext;
    protected readonly UnitOfWork UnitOfWork;

    protected virtual IQueryable<TEntity> DbSetIncludeProps => DbSet.AsQueryable();

    public Repository(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        dbContext = unitOfWork.Context;
        DbSet = unitOfWork.Context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return DbSetIncludeProps;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSetIncludeProps.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSetIncludeProps.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async virtual Task SaveAsync(TEntity entity, bool saveImmediately = true)
    {
        DbSet.Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;

        if (saveImmediately)
        {
            await UnitOfWork.SaveContext();
        }
    }

}
