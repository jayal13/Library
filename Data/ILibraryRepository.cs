using System.Linq.Expressions;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Data
{
    public interface ILibraryRepository
    {
        public TEntity? GetOneBy<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;
        public IEnumerable<TEntity> GetManyBy<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;        
        public IEnumerable<T> GetAll<T>() where T : class;
        public int EditOne<T>(T type, Action<T> mutate);
        public int AddOne<T>(T type);
        public int DeleteOne<T>(T type);
    }
}