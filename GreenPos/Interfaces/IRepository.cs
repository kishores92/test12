using GreenPOS.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GreenPOS.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<int> UpdateAsync(T t, object key);
        Task<int> AddAsync(T t);
        Task<int> DeleteAsync(T t);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(short id);
        Task<T> GetAsync(long id);
        Task<T> GetAsync(int id);
        IEnumerable<T> GetAll();
        T Get(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        //IQueryable<T> GetResultFromSql(string sqlQuery, params object[] parameters);
        Task<DbDataReader> ExecuteSQLAsync(string query, CommandType commandType, params object[] parameters);
        Task<T1> ExecuteSQLAndGetJsonResponseAsync<T1>(string storedProcedure, string entity, params SqlParameter[] parameters);
        Task<ApiResponse<T>> GetCurrentAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<int> DeleteAsync(long id);
    }
}
