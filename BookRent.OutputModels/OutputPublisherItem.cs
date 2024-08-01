using BookRent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookRent.OutputModels
{
    public class OutputPublisherItem
    {
        [JsonPropertyName ("id")]
        public string Id { get; set; }
        [JsonPropertyName ("name")]
        public string Name { get; set; }

        public OutputPublisherItem(Publisher publisher)
        {
            Id = publisher.Id.ToString();
            Name = publisher.Name;
        }
    }
}
