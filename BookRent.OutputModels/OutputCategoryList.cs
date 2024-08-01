using BookRent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookRent.OutputModels
{
    public class OutputCategoryList
    {
        [JsonPropertyName ("categories")]
        public ICollection<OutputCategoryItem> Categories {  get; set; }

        public OutputCategoryList (ICollection<Category> categories)
        {
            Categories = categories.Select(c => new OutputCategoryItem(c)).ToArray();
        }
    }
}
