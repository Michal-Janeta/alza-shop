using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AlzaShop.Core.Database;

public class UnitOfWork
{
    public AlzaShopDbContext Context;

    public UnitOfWork(AlzaShopDbContext context)
    {
        Context = context;
    }

    public void StartTransaction(IsolationLevel isolationLevel)
    {
        if (Context.Database.CurrentTransaction == null)
        {
            Context.Database.BeginTransaction(isolationLevel);
        }
    }

    public async Task SaveContext()
        => await Context.SaveChangesAsync();

    public void CommitTransaction()
    {
        if (Context.Database.CurrentTransaction != null)
        {
            Context.Database.CommitTransaction();
        }
    }
}
