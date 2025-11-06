using System.Data;
using AutoMapper;
using Library.Data;
using Library.Dtos;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LibraryController(ILibraryRepository bookRepository) : ControllerBase
{
    readonly ILibraryRepository _bookRepo = bookRepository;
    private readonly Mapper _mapper = new(new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<AddBookDto, Book>();
    }));

    [HttpGet("GetBooks/Id/{bookId}")]
    public Book GetBookById(int bookId)
    {
        return _bookRepo.GetOneBy<Book>(a => a.Id == bookId)
            ?? throw new KeyNotFoundException($"Book not found");
    }

    [HttpGet("GetBooks/Author/{author}")]
    public IEnumerable<Book> GetBookByAuthor(string author)
    {
        return _bookRepo.GetManyBy<Book>(a => a.Author == author)
            ?? throw new KeyNotFoundException($"Book not found");
    }

    [HttpGet("GetBooks/Title/{title}")]
    public Book GetBookByTitle(string title)
    {
        return _bookRepo.GetOneBy<Book>(a => a.Title == title)
            ?? throw new KeyNotFoundException($"Book not found");
    }

    [HttpGet("GetBooks")]
    public IEnumerable<Book> GetBooks()
    {
        return _bookRepo.GetAll<Book>();
    }

    [HttpPut("EditBook")]
    public IActionResult EditBook(Book newBook)
    {
        Book? book = _bookRepo.GetOneBy<Book>(a => a.Id == newBook.Id)
            ?? throw new KeyNotFoundException($"Book {newBook} not found");

        int rows = _bookRepo.EditOne<Book>(book, book =>
        {
            book.Title = newBook.Title;
            book.Author = newBook.Author;
            book.Pages = newBook.Pages;
            book.Availible = newBook.Availible;
        });
        return Ok("Updated " + rows + " rows");
    }

    [HttpPost("AddBook")]
    public IActionResult AddBook(AddBookDto newBook)
    {
        Book book = _mapper.Map<Book>(newBook);
        int rows = _bookRepo.AddOne<Book>(book);
        return Ok("Updated " + rows + " rows");
    }

    [HttpDelete("DeleteBook/{bookId}")]
    public IActionResult DeleteBook(int bookId)
    {
        Book? book = _bookRepo.GetOneBy<Book>(a => a.Id == bookId)
            ?? throw new KeyNotFoundException($"Book {bookId} not found");
        int rows = _bookRepo.DeleteOne<Book>(book);
        return Ok("Updated " + rows + " rows");
    }
}   
