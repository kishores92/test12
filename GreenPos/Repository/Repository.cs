using GreenPOS.Common;
using GreenPOS.Context;
using GreenPOS.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GreenPOSDBContext _context;
        private DbSet<T> _Entities;

        public Repository(GreenPOSDBContext context)
        {
            _context = context;
            _Entities = context.Set<T>();
        }

        public virtual async Task<int> UpdateAsync(T t, object key)
        {
            T m = await _context.Set<T>().FindAsync(key);
            if (m != null)
            {
                _context.Entry(m).CurrentValues.SetValues(t);
                var result = await _context.SaveChangesAsync();

                return result;
            }
            return -1;
        }

        public T Get(object id)
        {
            return _context.Find<T>(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _Entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _Entities.Remove(entity);
            _context.SaveChanges();
        }
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _Entities.Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        //public IQueryable<T> GetResultFromSql(string sqlQuery, params object[] parameters)
        //{
        //    var results = _context.Set<T>().FromSql(sqlQuery, parameters);
        //    var ss = results as SqlDataReader;
        //    return results;
        //}


        public async Task<DbDataReader> ExecuteSQLAsync(string query, CommandType commandType, params object[] parameters)
        {
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = query;
            cmd.CommandType = commandType;

            if (parameters.Any())
                cmd.Parameters.AddRange(parameters);

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            var result = await cmd.ExecuteReaderAsync();
            return result;
        }

        public async Task<T1> ExecuteSQLAndGetJsonResponseAsync<T1>(string storedProcedure, string entity, params SqlParameter[] parameters)
        {
            var paramsString = parameters.Any(a => a.ParameterName.Contains("@"))
                ? string.Join(",", parameters.Select(p => p.ParameterName))
                : string.Join(",", parameters.Select(p => $"@{p.ParameterName}"));
            var query = $"Exec {storedProcedure} {paramsString}";

            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;

            if (parameters.Any())
                cmd.Parameters.AddRange(parameters);

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            try
            {
                var result = await cmd.ExecuteReaderAsync();
                return Common.Extensions.GetJsonResponse<T1>(new StringBuilder(), result, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _Entities.AsEnumerable();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetAsync(short id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> AddAsync(T t)
        {
            _context.Set<T>().Add(t);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> DeleteAsync(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("entity");
            }
            _Entities.Remove(t);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ApiResponse<T>> GetCurrentAsync(Expression<Func<T, bool>> predicate)
        {
            var apiResponse = new ApiResponse<T> { Code = 0, Data = null, Message = "No Record Found" };
            var result = await _context.Set<T>().SingleOrDefaultAsync(predicate);
            if (result != null)
            {
                apiResponse.Code = 1;
                apiResponse.Message = "Success";
                apiResponse.Data = result;
            }

            return apiResponse;
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).FirstOrDefault();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public virtual async Task<int> DeleteAsync(long id)
        {
            var status = 0;
            var m = await _context.Set<T>().FindAsync(id);
            if (m != null)
            {
                _context.Set<T>().Remove(m);
                status = await _context.SaveChangesAsync();
            }
            return status;
        }
    }
}
