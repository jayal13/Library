using Microsoft.Identity.Client;

namespace Library.Dtos
{
    public class AddBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Pages { get; set; }
        public bool Availible { get; set; }
        public AddBookDto(){
            Title ??= "";
            Author ??= "";
        }
    }
}