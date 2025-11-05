using System.Data;
using Library.Data;
using Library.Dtos;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(IConfiguration config) : ControllerBase
{
    readonly DataContext _connection = new(config);

    [HttpGet("GetBooks/{bookId}")]
    public Book GetBook(int bookId)
    {
        Book? book = _connection.Books.Where(u => u.BookId == bookId).FirstOrDefault<Book>() 
        ?? throw new Exception($"Book {bookId} not found");
        return book;
    }

    [HttpGet("GetBooks")]
    public IEnumerable<Book> GetBooks()
    {
        IEnumerable<Book> books = [.. _connection.Books];
        return books;
    }

    [HttpPut("EditBook")]
    public IActionResult EditBook(Book newBook)
    {
        Book? book = _connection.Books.Where(u => u.BookId == newBook.BookId).FirstOrDefault<Book>()
        ?? throw new Exception($"Book {newBook.BookId} not found");

        book.Title = newBook.Title;
        book.Author = newBook.Author;
        book.Pages = newBook.Pages;
        book.Availible = newBook.Availible;
        
        int rows = _connection.SaveChanges();
        if ( rows == 0)
        {
            throw new Exception("Failed to Update book" + book.BookId);
        }
        return Ok("Updated " + rows + " rows");
    }

    [HttpPost("AddBook")]
    public IActionResult AddBook(AddBookDto newBook)
    {
        Book book = new()
        {
            Title = newBook.Title,
            Author = newBook.Author,
            Pages = newBook.Pages,
            Availible = newBook.Availible
        };
        _connection.Add(book);
        int rows = _connection.SaveChanges();
        if ( rows == 0)
        {
            throw new Exception("Failed to Update book" + book.BookId);
        }
        return Ok("Updated " + rows + " rows");
    }

    [HttpDelete("DeleteBook/{bookId}")]
    public IActionResult DeleteBook(int bookId)
    {
        Book? book = _connection.Books.Where(u => u.BookId == bookId).FirstOrDefault<Book>()
        ?? throw new Exception($"Book {bookId} not found");
        _connection.Remove(book);

        int rows = _connection.SaveChanges();
        if ( rows == 0)
        {
            throw new Exception("Failed to Update book" + book.BookId);
        }
        return Ok("Updated " + rows + " rows");
    }
}   
