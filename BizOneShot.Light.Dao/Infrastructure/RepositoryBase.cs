using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.DareConfiguration;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();

            //Eagar Loading을 사용하고자 하면 주석 풀면됨
            //DbContext.Configuration.LazyLoadingEnabled = false;
        }

        #region Properties

        private WebDbContext dbContext;
        private readonly IDbSet<T> dbSet;
        private IDbFactory dbFactory;

        protected IDbFactory DbFactory { get; }

        protected WebDbContext DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }

        #endregion

        #region 구현

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            //dbSet.Remove(entity);

            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetByKeys(IList<object> keys)
        {
            return dbSet.Find(keys.ToArray());
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        #endregion
    }

    public abstract class DareRepositoryBase<T> where T : class
    {
        protected DareRepositoryBase(IDareDbFactory dareDbFactory)
        {
            DareDbFactory = dareDbFactory;
            dbSet = DareDbContext.Set<T>();
        }

        #region Properties

        private DareDbContext dareDbContext;
        private readonly IDbSet<T> dbSet;
        private IDareDbFactory dbFactory;

        protected IDareDbFactory DareDbFactory { get; }

        protected DareDbContext DareDbContext
        {
            get { return dareDbContext ?? (dareDbContext = DareDbFactory.Init()); }
        }

        #endregion

        #region 구현

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dareDbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            //dbSet.Remove(entity);

            dbSet.Attach(entity);
            dareDbContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetByKeys(IList<object> keys)
        {
            return dbSet.Find(keys.ToArray());
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        #endregion
    }
}