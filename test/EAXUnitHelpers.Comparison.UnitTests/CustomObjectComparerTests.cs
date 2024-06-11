using System;
using System.Collections.Generic;

namespace EAXUnitHelpers.Comparison.Tests
{
    public class CustomObjectComparerTests
    {
        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheObjectsBeingComparedAreAPrimitiveTypeAndTheValuesAreDifferent()
        {
            // arrange
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"Bugs\"\r\nActual:   A value of \"Daffy\"";
            var comparer = new CustomObjectComparer<string>();

            // act
            var exception = Assert.Throws<EqualException>(() => comparer.Equals("Bugs", "Daffy"));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheObjectsBeingComparedAreADateTimeTypeAndTheValuesAreDifferent()
        {
            // arrange
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"6/9/2024 4:49:52 PM\"\r\nActual:   A value of \"6/9/2024 4:49:51 PM\"";
            var comparer = new CustomObjectComparer<DateTime>();
            var actual = new DateTime(2024, 06, 09, 16, 49, 51);
            var expected = new DateTime(2024, 06, 09, 16, 49, 52);

            // act
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomObjectComparerShouldResultInTrueWhenTheObjectsBeingComparedAreDateTimeTypeAndTheValuesAreTheSame()
        {
            // arrange
            var comparer = new CustomObjectComparer<DateTime>();
            var actual = new DateTime(2024, 06, 09, 16, 49, 51);
            var expected = new DateTime(2024, 06, 09, 16, 49, 51);

            // act
            var result = comparer.Equals(expected, actual);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheObjectsBeingComparedAreComplexAndDifferOnlyInAParentPropertyWhenParentPropertiesAreSupposedToBeChecked()
        {
            // arrange
            var actual = new Worker
            {
                Age = 45,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-45),
                FirstName = "Fifi",
                JobTitle = "Software Engineer",
                LastName = "LeGrange"
            };
            var expected = new Worker
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                JobTitle = "Software Engineer",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Worker>(true);
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"21\" for property \"Age\"\r\nActual:   A value of \"45\" for property \"Age\"";

            // act
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomObjectComparerShouldResultInTrueWhenTheObjectsBeingComparedAreComplexAndDifferOnlyInAParentPropertyWhenParentPropertiesAreNotSupposedToBeChecked()
        {
            // arrange
            var actual = new Worker
            {
                Age = 45,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-45),
                FirstName = "Fifi",
                JobTitle = "Software Engineer",
                LastName = "LeGrange"
            };
            var expected = new Worker
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                JobTitle = "Software Engineer",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Worker>();

            // act
            var result = comparer.Equals(expected, actual);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheObjectsBeingComparedAreComplexAndDifferOnlyInAPropertyThatShouldBeCheckedWhenAListOfPropertiesToCheckIsSpecified()
        {
            // arrange
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expected = new Person
            {
                Age = 49,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Person>(new List<string> { "Age" });
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"49\" for property \"Age\"\r\nActual:   A value of \"21\" for property \"Age\"";

            // act
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheObjectsBeingComparedAreComplexAndDifferOnlyInAPropertyThatShouldNotBeCheckedWhenAListOfPropertiesToCheckIsSpecified()
        {
            // arrange
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expected = new Person
            {
                Age = 49,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Person>(new List<string> { "ChangeInPocket", "DateOfBirth", "FirstName", "LastName" });

            // act
            var result = comparer.Equals(expected, actual);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomObjectComparerShouldResultInTrueWhenTheComparedObjectsAreEqualForPrimitiveTypesAtTheTopLevel()
        {
            // arrange
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expected = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Person>();

            // act
            var result = comparer.Equals(expected, actual);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomObjectComparerShouldThrowAnEqualExceptionWhenTheComparedObjectsAreNotEqualForPrimitiveTypesAtTheTopLevel()
        {
            // arrange
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expected = new Person
            {
                Age = 49,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Person>();

            // act/assert
            Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));
        }

        [Fact]
        public void CustomObjectComparerShouldIncludeTheCorrectErrorMessageWhenTheComparedObjectsAreNotEqualForPrimitiveTypesAtTheTopLevel()
        {
            // arrange
            var actual = new Person
            {
                Age = 21,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var expected = new Person
            {
                Age = 49,
                ChangeInPocket = .99m,
                DateOfBirth = DateTime.Today.AddYears(-21),
                FirstName = "Fifi",
                LastName = "LeGrange"
            };
            var comparer = new CustomObjectComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"49\" for property \"Age\"\r\nActual:   A value of \"21\" for property \"Age\"";

            // act
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}