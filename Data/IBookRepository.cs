using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Data
{
    public interface IBookRepository
    {
        public T GetOne<T>(int bookId) where T : class;
        public IEnumerable<T> GetAll<T>() where T : class;
        public int EditOne<T>(T type, Action<T> mutate);
        public int AddOne<T>(T type);
        public int DeleteOne<T>(T type);
    }
}