
using Microsoft.EntityFrameworkCore;
using WebApp.Data.ApplicationContext;

namespace WebApp.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _db;

        public DbSet<T> Set
        {
            get;
            private set;
        }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.LoadAsync();

            Set = set;
        }

        public async Task CreateAsync(T item)
        {
           await Set.AddAsync(item);
           await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            Set.Remove(item);

            await _db.SaveChangesAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await Set.FindAsync(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Set;
        }

        public async Task UpdateAsync(T item)
        {
            Set.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
