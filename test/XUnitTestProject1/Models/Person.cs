using System;
using System.Collections.Generic;

namespace EAXUnitHelpers.Comparison.Tests.Models
{
    public class Person
    {
        public int Age { get; set; }

        public decimal ChangeInPocket { get; set; }

        public IEnumerable<Person> Children { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Person Mother { get; set; }
    }
}