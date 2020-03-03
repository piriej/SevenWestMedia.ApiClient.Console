using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWestMedia.ApiClient.Console.ViewModels
{
    public class PeopleViewModel
    {
        public int Id { get; set; } = -1;
        public int Age { get; set; } = -1;
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { private get; set; } = "UNDEFINED";
        public string LastName { private get; set; } = "UNDEFINED";
        public List<string> FirstNames { get; set; } = new List<string>();
        public SortedList<int, GendersViewModel> GendersByAge { get; set; } = new SortedList<int, GendersViewModel>();

        // View
        public override string ToString()
        {
            var gendersByAge = Environment.NewLine + string.Join(Environment.NewLine, GendersByAge.Select(g => $"[ {g.Value} ]"));
            var firstNames = string.Join(", ", FirstNames);

            var fullNameText = Id == -1
                ? "There was no full name for the given Id." 
                : $"Full Name where id={Id}: {FullName}";
            var firstNamesText = Age == -1
                ? "There were no first names to display for the given age"
                : $"First Names where Age={Age}: {firstNames} ";

            var builder = new StringBuilder()
                .AppendLine(fullNameText)
                .AppendLine(firstNamesText)
                .AppendLine($"Genders By Age: {gendersByAge} ");

            return builder.ToString();
        }
    }
}