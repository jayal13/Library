using Microsoft.AspNetCore.Mvc;


namespace Library.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _connection;

        public BookRepository(IConfiguration config)
        {
            _connection = new DataContext(config);
        }

        public T GetOne<T>(int id) where T : class
        {
            T? resp = _connection.Set<T>().Find(id)
            ?? throw new Exception($"{id} not found");
            return resp;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            IEnumerable<T> resp = _connection.Set<T>().ToList();
            return resp;
        }

        public int EditOne<T>(T type, Action<T> mutate)
        {
            mutate(type);              // <- aquí cambias campos
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