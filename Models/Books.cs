using Microsoft.Identity.Client;

namespace Library.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Pages { get; set; }
        public bool Availible { get; set; }
    }
}