using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using APEXAContracting.DataAccess.DotNetCore.PagedList;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APEXAContracting.DataAccess.DotNetCore.Interface
{
    /// <summary>
    ///  Apply AutoMapper Querable Extension.
    ///  https://github.com/AutoMapper/AutoMapper/blob/master/docs/Queryable-Extensions.md
    ///  Reference: https://gist.github.com/johnpapa/3144387
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        #region CREATE
        /// <summary>
        ///  Add one new entity to table.
        /// </summary>
        /// <param name="entity"></param>
        EntityEntry<T> Add(T entity);

        /// <summary>
        ///  Add one new entity to table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ValueTask<EntityEntry<T>> AddAsync(T entity);


        /// <summary>
        ///  Add new entities to table.
        ///</summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        ///  Add new entities to table.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<T> entities);
        #endregion



        #region RETRIEVE
        /// <summary>
        /// Get all records from table.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();



        /// <summary>
        ///  Project DTO collection.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        IQueryable<D> GetAll<D>() where D : class;


        /// <summary>
        ///  Get all records from table. Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllNoTracking();



        /// <summary>
        ///  Project DTO collection.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        IQueryable<D> GetAllNoTracking<D>() where D : class;


        /// <summary>
        ///  Get one record based on primary key. Key is integer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        ///  Get one DTO object.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        D GetById<D>(int id) where D : class;

        /// <summary>
        ///  Get By Id Async.
        /// </summary>
        /// <param name="Id">Primary key. Integer.</param>
        /// <returns>Return one entity</returns>
        Task<T> GetByIdAsync(int Id);

        /// <summary>
        ///  Get one DTO object.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<D> GetByIdAsync<D>(int Id) where D : class;


        /// <summary>
        ///  Get one record based on primary key. Key is long integer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id);

        /// <summary>
        ///  Get one DTO object with long int primary key. 
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        D GetById<D>(long id) where D : class;

        /// <summary>
        ///  Get By Id Async.
        /// </summary>
        /// <param name="Id">Primary key. Long Integer.</param>
        /// <returns>Return one entity</returns>
        Task<T> GetByIdAsync(long Id);

        /// <summary>
        ///  Get one DTO object.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<D> GetByIdAsync<D>(long Id) where D : class;

        /// <summary>
        ///  Get one record based on primary key. Key is guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Guid id);

        /// <summary>
        ///  Return one DTO object.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        D GetById<D>(Guid id) where D : class;

        /// <summary>
        ///  Get entity by primary key. Guid.
        /// </summary>
        /// <param name="id">Uniqueidentifier primary key.</param>
        /// <returns>One entity.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        ///  Return one DTO object.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="id">Primary key. Uniqueidentifier.</param>
        /// <returns>One DTO object.</returns>
        Task<D> GetByIdAsync<D>(Guid id) where D : class;

        #region Query
        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query).
        /// </summary>
        /// <param name="filter">Filter conditions for "where" (SQL Query).</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        ///  Return DTO object IQuerable collection.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        IQueryable<D> Query<D>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;



        /// <summary>
        ///  Return entity object IQuerable collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        ///  Return DTO object collection. 
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        Task<List<D>> QueryAsync<D>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;


        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query).
        ///  Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);


        /// <summary>
        ///   Query and return DTO IQuerable collection.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        IQueryable<D> QueryNoTracking<D>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;



        /// <summary>
        /// Query entities collection with no tracking. Async.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        Task<List<T>> QueryNoTrackingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        /// Query DTO collecton with no tracking. Async.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Searching condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <returns></returns>
        Task<List<D>> QueryNoTrackingAsync<D>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;



        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query). Also allows to include specified collections.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        /// Query DTO objects with condition.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include"></param>
        /// <returns></returns>
        IQueryable<D> Query<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;


        /// <summary>
        ///  Query entity collection.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        ///  Query DTO collection.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        Task<List<D>> QueryAsync<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;




        /// <summary>
        ///  Search entities collection based on search conditions (SQL Query). Also allows to include specified collections.
        ///  Note: For query performance, disabling change tracking at the query level.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">include child properties.</param>
        /// <returns>IQuerable entity collection.</returns>
        IQueryable<T> QueryNoTracking(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);



        /// <summary>
        ///  Query DTO collection with no tracking.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        IQueryable<D> QueryNoTracking<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;


        /// <summary>
        /// Query Entity with no tracking. Async.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        Task<List<T>> QueryNoTrackingAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);


        /// <summary>
        /// Query DTO object with no tracking. Async.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="filter">Searching condition.</param>
        /// <param name="orderBy">Order by specified fields.</param>
        /// <param name="include">Specified include children. If Lazyloading disabled, need this setting.</param>
        /// <returns></returns>
        Task<List<D>> QueryNoTrackingAsync<D>(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;


        #endregion

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        #region FirstOrDefault
        /// <summary>
        /// Returns the first entity that matches the predicate else null
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <param name="orderBy"></param>
        /// <returns>An entity matching the predicate else null</returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
             

        /// <summary>
        ///  Get one DTO object.
        /// </summary>
        /// <typeparam name="D">DTO object.</typeparam>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <returns>Return one DTO object.</returns>
        D FirstOrDefault<D>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;

      

        /// <summary>
        ///  Return one entity.
        /// </summary>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <returns>Return one entity.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

       
        /// <summary>
        ///  Get one DTO object.
        /// </summary>
        /// <typeparam name="D">Data transfer object.</typeparam>
        /// <param name="predicate">Search condition.</param>
        /// <param name="orderBy"></param>
        /// <returns>Return one DTO object.</returns>
        Task<D> FirstOrDefaultAsync<D>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;

       

        /// <summary>
        ///  Get first entity that matches the predicate else null. Also include sepcified properties.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

      

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        D FirstOrDefault<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;

      

        /// <summary>
        /// Get first entity with search condition. Async.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

       

        /// <summary>
        /// Get first DTO object with search condition. Async.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<D> FirstOrDefaultAsync<D>(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) where D : class;

        #endregion

        #endregion

        #region UPDATE
        /// <summary>
        ///  Update one entity in database.
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        #endregion

        #region DELETE
        /// <summary>
        ///  Delete one entity from database.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        /// <summary>
        ///  Delete one entity with integer primary key.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        ///  Delete one entity with uniqueidentifier primary key.
        /// </summary>
        /// <param name="Id">Primary key.</param>
        void Delete(Guid Id);

        /// <summary>
        ///  delete entities collection based on search conditions (SQL Query).
        /// </summary>
        /// <param name="filter">Search condition</param>
        void Delete(Expression<Func<T, bool>> filter = null);
        #endregion

        #region Count
        /// <summary>
        ///  Count by search condition of linq.
        /// </summary>
        /// <param name="filter">Search condition.</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> filter = null);

        /// <summary>
        ///  Count by search condition of linq. 
        /// </summary>
        /// <param name="filter">Searching condition.</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        #endregion

        #region Paged List. Paginnation feature.
        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedList<T> GetPagedList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                         int pageIndex = 0,
                                         int pageSize = 20,
                                         bool disableTracking = true);


        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedList<D> GetPagedList<D>(Expression<Func<T, bool>> predicate = null,
               Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                             int pageIndex = 0,
                                             int pageSize = 20,
                                             bool disableTracking = true) where D : class;


        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">IQuerable collection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedList<T> GetPagedList(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                              int pageIndex = 0,
                                              int pageSize = 20,
                                              bool disableTracking = true);


        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="query">Pregenerated Queryable item.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>

        IPagedList<D> GetPagedList<D>(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int pageIndex = 0,
                                          int pageSize = 20,
                                          bool disableTracking = true) where D : class;



        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///  A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedList<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null,
              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                    int pageIndex = 0,
                                                    int pageSize = 20,
                                                    bool disableTracking = true,
                                                    CancellationToken cancellationToken = default(CancellationToken));



        /// <summary>
        /// Corporate with AutoMapper to Project DTO collection.
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
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
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedList<D>> GetPagedListAsync<D>(Expression<Func<T, bool>> predicate = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                          int pageIndex = 0,
                                                          int pageSize = 20,
                                                          bool disableTracking = true,
                                                          CancellationToken cancellationToken = default(CancellationToken)) where D : class;


        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
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
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedList<T>> GetPagedListAsync(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
                  Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                        int pageIndex = 0,
                                                        int pageSize = 20,
                                                        bool disableTracking = true,
                                                        CancellationToken cancellationToken = default(CancellationToken));



        /// <summary>
        /// Corporate with AutoMapper to project DTO collection.
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
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
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>

        Task<IPagedList<D>> GetPagedListAsync<D>(IQueryable<T> query, Expression<Func<T, bool>> predicate = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken)) where D : class;



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
        IPagedList<D> GetPagedList<D>(Expression<Func<T, D>> selector,
                                                  Expression<Func<T, bool>> predicate = null,
                                                  Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                  int pageIndex = 0,
                                                  int pageSize = 20,
                                                  bool disableTracking = true) where D : class;


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
        Task<IPagedList<D>> GetPagedListAsync<D>(Expression<Func<T, D>> selector,
                                                             Expression<Func<T, bool>> predicate = null,
                                                             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                             int pageIndex = 0,
                                                             int pageSize = 20,
                                                             bool disableTracking = true,
                                                             CancellationToken cancellationToken = default(CancellationToken)) where D : class;
        #endregion

    }
}
