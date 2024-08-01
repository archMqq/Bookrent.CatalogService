using BookRent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookRent.OutputModels
{
    public class OutputAuthorItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [JsonPropertyName("patronymic")]
        public string Patronymic { get; set; }
        [JsonPropertyName("DateOfBirth")]
        public string DateOfBirth { get; set; }

        public OutputAuthorItem(Author author)
        {
            Id = author.Id.ToString();
            Name = author.Name;
            Surname = author.Surname;
            Patronymic = author.Patronymic;
            DateOfBirth = author.DateOfBirth.ToShortDateString();
        }
    }
}
