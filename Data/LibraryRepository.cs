using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Library.Data
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly DataContext _connection;

        public LibraryRepository(IConfiguration config)
        {
            _connection = new DataContext(config);
        }

        public TEntity? GetOneBy<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return _connection.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetManyBy<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return [.. _connection.Set<TEntity>()
                            .AsNoTracking()
                            .Where(predicate)];
        }


        public IEnumerable<T> GetAll<T>() where T : class
        {
            IEnumerable<T> resp = _connection.Set<T>().ToList();
            return resp;
        }

        public int EditOne<T>(T type, Action<T> mutate)
        {
            mutate(type);              
            int rows = _connection.SaveChanges();
            if (rows == 0)
            {
                throw new Exception("Failed to Update " + type);
            }
            return rows;
        }

        public int AddOne<T>(T type)
        {
            _connection.Add(type);
            int rows = _connection.SaveChanges();
            if ( rows == 0)
            {
                throw new Exception("Failed to Update " + type);
            }
            return rows;
        }
        
        public int DeleteOne<T>(T type)
        {
            _connection.Remove(type);

            int rows = _connection.SaveChanges();
            if (rows == 0)
            {
                throw new Exception("Failed to Delete " + type);
            }
            return rows;
        }
    }
}