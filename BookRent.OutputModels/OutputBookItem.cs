using BookRent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookRent.OutputModels
{
    public class OutputBookItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName ("name")] 
        public string Name { get; set; }
        [JsonPropertyName ("discription")]
        public string Description { get; set; }
        [JsonPropertyName ("authors")]
        public OutputAuthorList Authors { get; set; }
        [JsonPropertyName ("publisher")]
        public OutputPublisherItem Publisher { get; set; }
        [JsonPropertyName ("publicationNumber")]
        public string PublicationNumber { get; set; }
        [JsonPropertyName("publicationYear")]
        public string PublicationYear { get; set; }
        [JsonPropertyName ("categories")]
        public OutputCategoryList Categories { get; set; }
        [JsonPropertyName ("rentedTimes")]
        public string RentedTimes { get; set; }

        public OutputBookItem(Book book)
        {
            Id = book.Id.ToString();
            Name = book.Name;
            Description = book.Description;
            Authors = new OutputAuthorList(book.Authors);
            Publisher = new OutputPublisherItem(book.Publisher);
            PublicationNumber = book.PublicationNumber.ToString();
            PublicationYear = book.PublicationYear.ToString();
            Categories = new OutputCategoryList(book.Categories);
            RentedTimes = book.RentedTimes.ToString();
        }
    }
}
