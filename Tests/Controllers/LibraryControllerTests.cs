using Microsoft.Extensions.Configuration;
using Library.Controllers;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Library.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controllers
{
    internal class TestDataContext : DataContext
    {
        private readonly string _dbName;

        public TestDataContext(IConfiguration config, string dbName) : base(config)
        {
            _dbName = dbName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Siempre usar InMemory para los tests
            optionsBuilder.UseInMemoryDatabase(_dbName);
        }
    }
    public class LibraryControllerTests
    {
        private readonly TestDataContext _context;
        private readonly LibraryController _controller;

        public LibraryControllerTests()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
            _context = new TestDataContext(config, Guid.NewGuid().ToString());
            
            _context.Database.EnsureCreated();
            _context.Books.AddRange(
                new Book { BookId = 1, Title = "LOTR", Author = "Tolkien", Pages = 500, Availible = true },
                new Book { BookId = 2, Title = "Hobbit", Author = "Tolkien", Pages = 300, Availible = true }
            );
            _context.SaveChanges();

            _controller = new LibraryController(config);
            typeof(LibraryController)
                .GetField("_connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_controller, _context);
        }

        [Fact]
        public void GetBooks()
        {
            var result = _controller.GetBooks();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetBook()
        {
            var result = _controller.GetBook(1);
            Assert.Equal("LOTR", result.Title);
        }

        [Fact]
        public void GetBook_Failed()
        {
            Assert.Throws<Exception>(() => _controller.GetBook(99));
        }

        [Fact]
        public void AddBook()
        {
            var dto = new AddBookDto { Title = "LOTR", Author = "Tolkien", Pages = 00, Availible = true };

            var result = _controller.AddBook(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Contains("Updated 1 rows", result.Value!.ToString());
            Assert.Equal(3, _context.Books.Count());
        }

        [Fact]
        public void EditBook()
        {
            var updated = new Book { BookId = 1, Title = "LOTR", Author = "Tolkien", Pages = 550, Availible = true };

            var result = _controller.EditBook(updated) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Contains("Updated 1 rows", result.Value!.ToString());
            var book = _context.Books.Find(1);
            Assert.Equal(550, book!.Pages);
        }

        [Fact]
        public void DeleteBook()
        {
            var result = _controller.DeleteBook(1) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Contains("Updated", result!.Value!.ToString());
            Assert.Null(_context.Books.Find(1));
        }
    }
}