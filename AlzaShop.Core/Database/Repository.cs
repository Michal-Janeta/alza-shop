using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace AlzaShop.Core.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected DbSet<TEntity> DbSet;
    protected readonly AlzaShopDbContext dbContext;
    protected virtual IQueryable<TEntity> DbSetIncludeProps => DbSet.AsQueryable();

    public Repository(AlzaShopDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<TEntity>();
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
}
