using APEXAContracting.DataAccess.DotNetCore.PagedList;
using System;
using System.Collections.Generic;
using System.Text;
using APEXAContracting.DataAccess.DotNetCore.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APEXAContracting.DataAccess.DotNetCore.Repository
{
    /// <summary>
    ///  Generic repository for accessing database with EntityFramework core.
    ///  Reference: https://gist.github.com/johnpapa/3144387
    /// </summary>
    /// <typeparam name="T">It could be Entity Framework code first entity or POCO class.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Properties
        /// <summary>
        /// It represents one database connection.
        /// </summary>
        protected BaseDbContext DbContext { get; set; }

        /// <summary>
        ///  Represent one database.
        /// </summary>
        protected DbSet<T> DbSet { get; set; }

        /// <summary>
        ///  Auto Mapper Interface. Assigned by dependency injection from consturctor.
        /// </summary>
        protected IMapper Mapper { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Corporate with dependency injection with DbContext and AutoMapper if required.
        /// </summary>
        /// <param name="dbContext">Database connection instance. Dependency injection.</param>
        /// <param name="mapper">Auto mapper dependency injection.</param>
        public BaseRepository(DbContext dbContext, IMapper mapper = null)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext is required in BaseRepository.");
            }

            this.DbContext = dbContext as BaseDbContext;
            this.DbSet = dbContext.Set<T>();
            this.Mapper = mapper;
        }
        #endregion

        #region CREATE
        /// <summary>
        ///  Add one new entity to table.
        /// </summary>
        /// <param name="entity"></param>
        public virtual EntityEntry<T> Add(T entity)
        {
            //var dbEntityEntry = DbContext.Entry(entity);
            //if (dbEntityEntry.State != EntityState.Detached)
            //{
            //    dbEntityEntry.State = EntityState.Added;
            //}
            //else
            //{
            //    DbSet.Add(entity);
            //}

            return DbSet.Add(entity);
        }

        /// <summary>
        ///  Add new entity async.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValueTask<EntityEntry<T>> AddAsync(T entity)
        {
            return this.DbSet.AddAsync(entity);
        }

        /// <summary>
        ///  Add new entities to table.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            this.DbSet.AddRange(entities);
        }

        /// <summary>
        ///  Add new entities to table.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task AddRangeAsync(IEnumerable<T> entities)
        {

            return this.DbSet.AddRangeAsync(entities);
        }

        #endregion

        #region RETRIEVE
        /// <summary>
        ///  Get all records from table.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }


        /// <summary>
        ///  Get all records and binding to DTO objects.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        public IQueryable<D> GetAll<D>() where D : class
        {
            return this.DbSet.ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }

       
        /// <summary>
        ///  Get all records from table. Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAllNoTracking()
        {
            return this.DbSet.AsNoTracking();
        }

      

        /// <summary>
        ///  Get all records and project to DTO entities.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        public IQueryable<D> GetAllNoTracking<D>() where D : class
        {
            return this.DbSet.AsNoTracking().ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }


        /// <summary>
        ///  Get one entity object with int primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        ///  Get one entity object by long primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        ///  Get one entity with uniqueidentifier primary key.
        /// </summary>
        /// <param name="id">Primary key. Uniqueidentifier.</param>
        /// <returns></returns>
        public T GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Get one DTO object with int primary key.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key.</param>
        /// <returns></returns>
        public D GetById<D>(int id) where D : class
        {
            return this.Mapper.Map<T, D>(DbSet.Find(id));
        }

        /// <summary>
        /// Get one DTO object with long int primary key.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key.</param>
        /// <returns></returns>
        public D GetById<D>(long id) where D : class
        {
            return this.Mapper.Map<T, D>(DbSet.Find(id));
        }

        /// <summary>
        /// Get one DTO object with primary key.
        /// Note: This call required auto mapper register.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key. Uniqueidentifier.</param>
        /// <returns></returns>
        public D GetById<D>(Guid id) where D : class
        {
            T result = DbSet.Find(id);

            if (result != null)
                return this.Mapper.Map<T, D>(result);
            else
                return null;
        }


        /// <summary>
        /// Get one entity with primary key.
        /// </summary>
        /// <param name="id">Primary key. Integer.</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Get one DTO object with primary key.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key. Integer.</param>
        /// <returns></returns>
        public async Task<D> GetByIdAsync<D>(int id) where D : class
        {
            return this.Mapper.Map<T, D>(await DbSet.FindAsync(id));
        }


        /// <summary>
        /// Get one entity with long int primary key.
        /// </summary>
        /// <param name="id">Primary key. Integer.</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Get one DTO object with long int primary key.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key. Integer.</param>
        /// <returns></returns>
        public async Task<D> GetByIdAsync<D>(long id) where D : class
        {
            return this.Mapper.Map<T, D>(await DbSet.FindAsync(id));
        }

        /// <summary>
        /// Get one entity with primary key.
        /// </summary>
        /// <param name="id">Primary key. Uniqueidentifier.</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await this.DbSet.FindAsync(id);
        }

        /// <summary>
        /// Get one DTO object with primary key. Async.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key.</param>
        /// <returns></returns>
        public async Task<D> GetByIdAsync<D>(Guid id) where D : class
        {
            return this.Mapper.Map<T, D>(await this.DbSet.FindAsync(id));
        }

        #region Query

        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query).
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }


    
        /// <summary>
        /// Query DTO collection.
        /// </summary>
        /// <typeparam name="D">DTO collection.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<D> Query<D>(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }

       

        /// <summary>
        ///  Query and convert to list of objects in an asynchronous call.
        ///  Note: This will return List instead of IQueryable.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

               
        /// <summary>
        ///  Query DTO collection async.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<D>> QueryAsync<D>(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ProjectTo<D>(this.Mapper.ConfigurationProvider).ToListAsync();
        }
               


        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query).
        ///  Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsNoTracking();
        }
               

        /// <summary>
        /// Query DTO collection.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<D> QueryNoTracking<D>(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsNoTracking().ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }
        

        /// <summary>
        /// Query Entity collection with no tracking. Async.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<T>> QueryNoTrackingAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }
              

        /// <summary>
        /// Query DTO collection with no tracking. Async.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<D>> QueryNoTrackingAsync<D>(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ProjectTo<D>(this.Mapper.ConfigurationProvider).ToListAsync();
        }
             


        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query). Also allows to include specified collections.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include">include child properties.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
                        

        /// <summary>
        /// Query DTO collection.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<D> Query<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }

      
        /// <summary>
        /// Query entity collection. Async.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specifield fields.</param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            )
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }


        /// <summary>
        ///  Query DTO collection. Async.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<D>> QueryAsync<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ProjectTo<D>(this.Mapper.ConfigurationProvider).ToListAsync();
        }




        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query). Also allows to include specified collections.
        ///  Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include">include child properties.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            )
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsNoTracking();
        }




        /// <summary>
        ///  Query DTO collection with no tracking.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public IQueryable<D> QueryNoTracking<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsNoTracking().ProjectTo<D>(this.Mapper.ConfigurationProvider);
        }

       

        /// <summary>
        ///  Query entity collection with no tracking. Async.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specified field.</param>
        /// <returns></returns>
        public async Task<List<T>> QueryNoTrackingAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            )
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }


    
        /// <summary>
        ///  Query DTO collection with no tracking. Async.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        public async Task<List<D>> QueryNoTrackingAsync<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            ) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ProjectTo<D>(this.Mapper.ConfigurationProvider).ToListAsync();
        }

 
        #endregion

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Any(predicate);
        }

        #region FirstOrDefault

        /// <summary>
        /// Returns the first entity that matches the predicate else null
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <param name="orderBy"></param>
        /// <returns>An entity matching the predicate else null</returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.FirstOrDefault(predicate);
        }
        
     

        /// <summary>
        ///  This method required mapper register.
        /// </summary>
        /// <typeparam name="D">DTO object. Data Transfer object.</typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public D FirstOrDefault<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return this.Mapper.Map<T, D>(query.FirstOrDefault(predicate));
        }
            


        /// <summary>
        ///  Return first entity with asynchronous.
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="predicate">Search condition.</param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
                 

        /// <summary>
        ///  Get first DTO object with search condition.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<D> FirstOrDefaultAsync<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return this.Mapper.Map<T, D>(await query.FirstOrDefaultAsync(predicate));
        }

        /// <summary>
        ///  Get first entity that matches the predicate else null. Also include sepcified properties.
        /// </summary>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
           )
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.FirstOrDefault(predicate);
        }

            


        /// <summary>
        ///  Get first DTO object with search condition.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="predicate">Searching condition.</param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public D FirstOrDefault<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            ) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return this.Mapper.Map<T, D>(query.FirstOrDefault(predicate));
        }
         

        /// <summary>
        ///  Get first entity with search condition. Async.
        /// </summary>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }


        /// <summary>
        ///  Get first DTO object with search condition. Async.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<D> FirstOrDefaultAsync<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            ) where D : class
        {
            IQueryable<T> query = this.DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return this.Mapper.Map<T, D>(await query.FirstOrDefaultAsync(predicate));
        }

  
        #endregion

        #endregion

        #region UPDATE
        /// <summary>
        ///  Update one entity in database.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        #endregion

        #region DELETE
        /// <summary>
        ///  Delete one entity with primary key.
        /// </summary>
        /// <param name="id">Parimey key. Uniqueidentifier.</param>
        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
        }

        /// <summary>
        ///  Delete one entity with parimary key.
        /// </summary>
        /// <param name="id">Primary key. Integer.</param>
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
        }


        /// <summary>
        ///  Delete one entity from database.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public void Delete(T entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }


        /// <summary>
        ///  delete entities collection based on search conditions (SQL Query).
        /// </summary>
        /// <param name="filter">Search condition.</param>
        public void Delete(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = this.DbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var item in query)
            {
                this.Delete(item);
            }
        }

        #endregion

        #region Count
        /// <summary>
        ///  Count by search condition of linq.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        /// <summary>
        ///  Count by search condition of linq. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
        #endregion

        #region Paged List
        /// <summary>
        /// Gets the <see cref="IPagedList{T}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IPagedList<T> GetPagedList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true)
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IPagedList<D> GetPagedList<D>(Expression<Func<T, bool>> predicate,
             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                             int pageIndex = 0,
                                             int pageSize = 20,
                                             bool disableTracking = true) where D : class
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList<D, T>(this.Mapper, pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList<D, T>(this.Mapper, pageIndex, pageSize);
            }
        }


        /// <summary>
        /// Gets the <see cref="IPagedList{T}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">Pregenerated Queryable item.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IPagedList<T> GetPagedList(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                              int pageIndex = 0,
                                              int pageSize = 20,
                                              bool disableTracking = true)
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">Pregenerated Queryable item.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>

        public IPagedList<D> GetPagedList<D>(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int pageIndex = 0,
                                          int pageSize = 20,
                                          bool disableTracking = true) where D : class
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList<D, T>(this.Mapper, pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList<D, T>(this.Mapper, pageIndex, pageSize);
            }
        }


        /// <summary>
        /// Gets the <see cref="IPagedList{T}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public Task<IPagedList<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null,
              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public Task<IPagedList<D>> GetPagedListAsync<D>(Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                          int pageIndex = 0,
                                                          int pageSize = 20,
                                                          bool disableTracking = true,
                                                          CancellationToken cancellationToken = default(CancellationToken)) where D : class
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync<D, T>(this.Mapper, pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync<D, T>(this.Mapper, pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{T}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">IQuerable entity collection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>

        public Task<IPagedList<T>> GetPagedListAsync(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }


        /// <summary>
        /// Corporate with AutoMapper to project DTO collection.
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">IQuerable collection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>

        public Task<IPagedList<D>> GetPagedListAsync<D>(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken)) where D : class
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync<D, T>(this.Mapper, pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync<D, T>(this.Mapper, pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IPagedList<D> GetPagedList<D>(Expression<Func<T, D>> selector,
                                                         Expression<Func<T, bool>> predicate = null,
                                                         Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,

                                                         int pageIndex = 0,
                                                         int pageSize = 20,
                                                         bool disableTracking = true)
            where D : class
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.Select(selector).ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{D}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{D}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public Task<IPagedList<D>> GetPagedListAsync<D>(Expression<Func<T, D>> selector,
                                                                    Expression<Func<T, bool>> predicate = null,
                                                                    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                    int pageIndex = 0,
                                                                    int pageSize = 20,
                                                                    bool disableTracking = true,
                                                                    CancellationToken cancellationToken = default(CancellationToken))
            where D : class
        {
            IQueryable<T> query = this.DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }

        #endregion
    }
}
