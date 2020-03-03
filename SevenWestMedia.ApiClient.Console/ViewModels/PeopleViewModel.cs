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

        public override string ToString()
        {
            var gendersByAge = string.Join(Environment.NewLine, GendersByAge.Select(g => g.ToString()));

            var builder = new StringBuilder()
                .AppendLine($"Full Name where id={Id}: {FullName} ")
                .AppendLine($"First Names where Age={Age}: {FirstNames} ")
                .AppendLine($"Genders By Age: {gendersByAge} ");

            return builder.ToString();
        }
    }
}