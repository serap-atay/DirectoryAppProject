using DirectoryApp.Data;
using DirectoryApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace DirectoryApp.Repository.Abstracts
{
    public abstract class RepositoryBase<T, TId> : IRepository<T, TId>
        where T : BaseEntity, new()
        {
            protected MyContext _context;
            public DbSet<T> Table { get; protected set; }
            protected RepositoryBase(MyContext myContext)
            {
                _context = myContext;
                Table = _context.Set<T>();
            }
            public virtual T GetById(TId id)
            {
                return Table.Find(id);
            }
            public virtual IQueryable<T> Get(Func<T, bool> predicate = null)
            {
                return predicate == null ? Table : Table.Where(predicate).AsQueryable();
            }

            public virtual IQueryable<T> Get(string[] includes, Func<T, bool> predicate = null)
            {
                IQueryable<T> query = Table;
                foreach (var include in includes)
                {
                    query = Table.Include(include);
                }
                return predicate == null ? query : query.Where(predicate).AsQueryable();
            }

            public virtual void Add(T entity, bool isSaveLater = false)
            {
                Table.Add(entity);
                if (!isSaveLater)
                    this.Save();

            }
            public virtual void Remove(T entity, bool isSaveLater = false)
            {
                Table.Remove(entity);
                if (!isSaveLater)
                    this.Save();
            }

            public virtual void Update(T entity, bool isSaveLater = false)
            {
                Table.Update(entity);
                if (!isSaveLater)
                    this.Save();
            }
            public virtual int Save()
            {
            foreach (var entry in   _context.ChangeTracker.Entries<Contact>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.Entity.isDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                    case EntityState.Modified:
                        break;
                    case EntityState.Added:
                        entry.Entity.isDeleted = false;
                        break;
                    default:
                        break;
                }
            }
            return _context.SaveChanges();
            }
        }
}
