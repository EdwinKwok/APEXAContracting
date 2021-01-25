using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace APEXAContracting.DataAccess.DotNetCore.PagedList
{
    /// <summary>
    ///  Paginnation process based on IQueryable collection. Corporate with EntityFrameworkCore.
    ///  
    ///  While querying data from database, IQueryable executes select query on server side with all filters.
    ///  Hence does less work and becomes fast.
    /// </summary>
    public static class IQueryablePageListExtensions
    {
        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// Note: IQueryable{T} is linq which hasn't instanced as physically existing collection yet.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }

        /// <summary>
        ///  Corporate with AutoMapper's QueryableExtension to paginnation DTO collection in SQL.
        ///  Apply ProjectTo{D}.
        /// </summary>
        /// <typeparam name="D">It is DTO (Data Transfer Object) which will be exposed to Web API as Json.</typeparam>
        /// <typeparam name="E">It is Entity Models which references to tables.</typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexFrom"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPagedList<D>> ToPagedListAsync<D, E>(this IQueryable<E> source, IMapper mapper, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize).ProjectTo<D>(mapper.ConfigurationProvider).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<D>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// Note: IQueryable{T} is linq which hasn't instanced as physically existing collection yet.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = source.Count();
            var items = source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToList();

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }

        /// <summary>
        ///  Corporate with AutoMapper's QueryableExtension to paginnation DTO collection in SQL.
        ///  Apply ProjectTo{D}.
        /// </summary>
        /// <typeparam name="D">It is DTO (Data Transfer Object) which will be exposed to Web API as Json.</typeparam>
        /// <typeparam name="E">It is Entity Models which references to tables.</typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexFrom"></param>
        /// <returns></returns>
        public static IPagedList<D> ToPagedList<D, E>(this IQueryable<E> source, IMapper mapper, int pageIndex, int pageSize, int indexFrom = 0)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = source.Count();
            var items = source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ProjectTo<D>(mapper.ConfigurationProvider).ToList();

            var pagedList = new PagedList<D>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }


    }
}
