using System.Data;
using AutoMapper;
using Library.Data;
using Library.Dtos;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(IBookRepository bookRepository) : ControllerBase
{
    readonly IBookRepository _bookRepo = bookRepository;
    private readonly Mapper _mapper = new(new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<AddBookDto, Book>();
    }));

    [HttpGet("GetBooks/{bookId}")]
    public Book GetBook(int bookId)
    {
        return _bookRepo.GetOne<Book>(bookId);
    }

    [HttpGet("GetBooks")]
    public IEnumerable<Book> GetBooks()
    {
        return _bookRepo.GetAll<Book>();
    }

    [HttpPut("EditBook")]
    public IActionResult EditBook(Book newBook)
    {
        Book? book = _bookRepo.GetOne<Book>(newBook.Id);

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
        Book? book = _bookRepo.GetOne<Book>(bookId);
        int rows = _bookRepo.DeleteOne<Book>(book);
        return Ok("Updated " + rows + " rows");
    }
}   
