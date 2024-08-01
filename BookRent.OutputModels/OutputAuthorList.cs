using BookRent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookRent.OutputModels
{
    public class OutputAuthorList
    {
        [JsonPropertyName("authors")]
        public ICollection<OutputAuthorItem> Authors {  get; set; }

        public OutputAuthorList(ICollection<Author> authors) 
        {
            Authors = authors.Select(x => new OutputAuthorItem(x)).ToArray();
        }
    }
}
