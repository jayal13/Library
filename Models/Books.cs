using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Pages { get; set; }
        public bool Availible { get; set; }
    }
}