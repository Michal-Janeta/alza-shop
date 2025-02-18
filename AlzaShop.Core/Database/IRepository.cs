namespace AlzaShop.Core.Database;

public interface IRepository<TEntity> where TEntity : class, new()
{
    IQueryable<TEntity> GetQueryable();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
}
