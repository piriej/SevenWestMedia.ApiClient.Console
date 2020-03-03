using System.Collections.Generic;
using System.Text;

namespace SevenWestMedia.ApiClient.Console.ViewModels
{
    public class GendersViewModel
    {
        public int Age { get; set; }
        public Dictionary<string, int> Genders { get; set; } = new Dictionary<string, int>();

        private string GenderMap(string gender) => gender switch
        {
            "M" => "Male",
            "F" => "Female",
            _ => gender
        };

        public override string ToString()
        {
            var builder = new StringBuilder()
                .Append($"Age: {Age} ");

            foreach (var gender in Genders)
            {
                var key = GenderMap(gender.Key);

                builder.Append($", {key}: ${gender.Value}");
            }

            builder.AppendLine();
            return builder.ToString();
        }
    }
}