using Newtonsoft.Json;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library
{
    public class Person : IModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "first")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
    }
}