using System.Collections.Generic;
using SevenWestMedia.ApiClient.Library.Models;

namespace SevenWestMedia.ApiClient.Library.Test
{
    public static class PeopleMock
    {
        public static List<Person> WithMultipleGendersAndAges()
        {
            return new List<Person>
            {
                new Person {Id = 1, FirstName = "Jordie", LastName = "La Forge", Gender = "M", Age = 12},
                new Person {Id = 2, FirstName = "Jean-Luc", LastName = "Picard", Gender = "M", Age = 22},
                new Person {Id = 3, FirstName = "William", LastName = "Riker", Gender = "M", Age = 22},
                new Person {Id = 3, FirstName = "Deanna", LastName = "Troy", Gender = "F", Age = 22},
                new Person {Id = 4, FirstName = "Seven", LastName = "Of Nine", Gender = "X", Age = 23}
            };
        }

    }
}