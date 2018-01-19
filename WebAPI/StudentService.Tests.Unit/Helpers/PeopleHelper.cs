using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using StudentService.DTOs;
using StudentService.Controllers;
using StudentService.Data;
using Moq;

namespace StudentService.Tests.Unit.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class PeopleHelper
    {
        public static List<Person> GetPeopleList()
        {

            var people = new List<Person>
            {
                new Person { FirstName = "Amber", LastName = "Baar", PersonID = 3 },
                new Person { FirstName = "Ben", LastName = "Xavier", PersonID = 1 },
                new Person { FirstName = "Ronald", LastName = "Weasley", PersonID = 5 }

            };

            return people;
        }

        public static List<PersonDTO> GetPeopleListDTO()
        {

            var people = new List<PersonDTO>
            {
                new PersonDTO { FirstName = "Amber", LastName = "Baar", PersonID = 3 },
                new PersonDTO { FirstName = "Ben", LastName = "Xavier", PersonID = 1 },
                new PersonDTO { FirstName = "Ronald", LastName = "Weasley", PersonID = 5 }

            };

            return people;
        }
    }
}
