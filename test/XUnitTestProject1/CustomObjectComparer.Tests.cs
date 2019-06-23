using EAXUnitHelpers.Comparison;
using EAXUnitHelpers.Comparison.Tests.Models;
using System;
using Xunit;
using Xunit.Sdk;

namespace XUnitTestProject1
{
    public class CustomObjectComparerTests
    {
        [Fact]
        public void CustomObjectComparerShouldComparePropertyValuesForPrimitiveTypesAtTheBottomLevel()
        {
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expectedToMatch = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expectedNotToMatch = new Person
            {
                Age = 49,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };

            var comparer = new CustomObjectComparer<Person>();
            try
            {
                var result = comparer.Equals(expectedToMatch, actual);
                Assert.True(result);
            }
            catch (EqualException)
            {
                Assert.False(true);
            }
            try
            {
                var result = comparer.Equals(expectedNotToMatch, actual);
                Assert.True(false);
            }
            catch (EqualException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void CustomObjectComparerShouldComparePropertyValuesForPrimitiveTypesOnCustomObjectsAtTheBottomLevel()
        {
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange",
                Mother = new Person
                {
                    Age = 37
                }
            };
            var expectedToMatch = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange",
                Mother = new Person
                {
                    Age = 37
                }
            };
            var expectedNotToMatch = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange",
                Mother = new Person
                {
                    Age = 39
                }
            };

            var comparer = new CustomObjectComparer<Person>();
            try
            {
                var result = comparer.Equals(expectedToMatch, actual);
                Assert.True(result);
            }
            catch (EqualException)
            {
                throw;
            }
            try
            {
                var result = comparer.Equals(expectedNotToMatch, actual);
                Assert.True(false);
            }
            catch (EqualException)
            {
                Assert.True(true);
            }
        }
    }
}
