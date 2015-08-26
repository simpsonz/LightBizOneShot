using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.DareConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        #region Properties
        private WebDbContext dbContext;
        private readonly IDbSet<T> dbSet;
        private IDbFactory dbFactory;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }



        protected WebDbContext DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }



        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }



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
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
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

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion
    }

    public abstract class DareRepositoryBase<T> where T : class
    {
        #region Properties
        private DareDbContext dareDbContext;
        private readonly IDbSet<T> dbSet;

        protected IDareDbFactory DareDbFactory
        {
            get;
            private set;
        }

        protected DareDbContext DareDbContext
        {
            get { return dareDbContext ?? (dareDbContext = DareDbFactory.Init()); }
        }
        #endregion

        protected DareRepositoryBase(IDareDbFactory dareDbFactory)
        {
            DareDbFactory = dareDbFactory;
            dbSet = DareDbContext.Set<T>();
        }

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
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
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

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion
    }
}
