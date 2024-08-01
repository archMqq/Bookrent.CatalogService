using BookRent.Models;
using System.Text.Json.Serialization;

namespace BookRent.OutputModels
{
    public class OutputBookList
    {
        [JsonPropertyName ("books")]
        public ICollection<OutputBookItem> Books { get; set; }

        public OutputBookList(ICollection<Book> books)
        {
            Books = books.Select(b => new OutputBookItem(b)).ToArray();
        }
    }
}
