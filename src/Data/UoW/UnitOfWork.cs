using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApp.Data.ApplicationContext;
using WebApp.Data.Repository;

namespace WebApp.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {   
        private readonly ApplicationDbContext _appContext;

        private Dictionary<Type, object> repositories;

        public UnitOfWork(ApplicationDbContext appContext)
        {
            _appContext = appContext;
        }

        public void Dispose()
        {
        
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository) where TEntity : class
        {
            if(repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            if(hasCustomRepository)
            {
                var customRepo = _appContext.GetService<IRepository<TEntity>>();
                if(customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity); 
            if(!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_appContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public int SaveChanges(bool ensureAutoHistory)
        {
            throw new NotImplementedException();
        }
    }
}
