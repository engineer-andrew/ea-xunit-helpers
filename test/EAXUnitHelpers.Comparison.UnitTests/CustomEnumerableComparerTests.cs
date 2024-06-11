using System;
using System.Collections.Generic;

namespace EAXUnitHelpers.Comparison.UnitTests
{
    public class CustomEnumerableComparerTests
    {
        [Fact]
        public void CustomEnumerableComparerShouldResultInTrueWhenBothEnumerablesAreNull()
        {
            // arrange
            var comparer = new CustomEnumerableComparer<Person>();

            // act
            var result = comparer.Equals(null, null);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheExpectedEnumerableIsNullAndTheActualEnumerableIsNot()
        {
            // arrange
            var actual = new List<Person>
            {
                new()
                {
                    Age = 49,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var comparer = new CustomEnumerableComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A null collection\r\nActual:   A collection containing 1 objects";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(null, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenTheExpectedEnumerableIsNotNullAndTheActualEnumerableIs()
        {
            // arrange
            var expected = new List<Person>
            {
                new()
                {
                    Age = 49,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var comparer = new CustomEnumerableComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A collection containing 1 objects\r\nActual:   A null collection";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, null));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenNeitherEnumerableIsNullButTheyHaveADifferentNumberOfObjects()
        {
            // arrange
            var actual = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var expected = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                },
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var comparer = new CustomEnumerableComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: 2 items\r\nActual:   1 items";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnEqualExceptionWithTheCorrectErrorMessageWhenNeitherEnumerableIsNullButTheyHaveADifferentNumberOfObjectsInOneOfTheirChildProperties()
        {
            // arrange
            var actual = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    Children =
                    [
                        new()
                        {
                            Age = 1,
                            DateOfBirth = DateTime.Today.AddYears(-1),
                            FirstName = "Baby",
                            LastName = "LeGrange"
                        }
                    ],
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var expected = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    Children = [],
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var comparer = new CustomEnumerableComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: 0 items in Children\r\nActual:   1 items in Children";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldResultInTrueWhenBothEnumerablesAreEmpty()
        {
            // arrange
            var actual = new List<Person>();
            var expected = new List<Person>();
            var comparer = new CustomEnumerableComparer<Person>();

            // act
            var result = comparer.Equals(expected, actual);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnExceptionWithTheCorrectErrorMessageWhenTheEnumerableTypeIsStringAndOneItemIsDifferent()
        {
            // arrange
            var actual = new List<string> { "Bugs", "Daffy", "Pork", "Elmer" };
            var expected = new List<string> { "Bugs", "Daffy", "Porky", "Elmer" };
            var comparer = new CustomEnumerableComparer<string>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"Porky\" in position 2\r\nActual:   A value of \"Pork\" in position 2";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnExceptionWithTheCorrectErrorMessageWhenTheEnumerableTypeIsStringAndTheItemsAreNotInTheSameOrder()
        {
            // arrange
            var actual = new List<string> { "Bugs", "Porky", "Daffy", "Elmer" };
            var expected = new List<string> { "Bugs", "Daffy", "Porky", "Elmer" };
            var comparer = new CustomEnumerableComparer<string>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"Daffy\" in position 1\r\nActual:   A value of \"Porky\" in position 1";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnExceptionWithTheCorrectErrorMessageWhenTheEnumerableTypeIsIntAndOneItemIsDifferent()
        {
            // arrange
            var actual = new List<int> { 8, 7, 6, 5, 3, 0, 9 };
            var expected = new List<int> { 8, 6, 7, 5, 3, 0, 9 };
            var comparer = new CustomEnumerableComparer<int>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"6\" in position 1\r\nActual:   A value of \"7\" in position 1";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void CustomEnumerableComparerShouldThrowAnExceptionWithTheCorrectErrorMessageWhenOneOfThePropertiesInOneOfTheEnumerablesIsDifferentFromTheSamePropertyOnTheObjectInTheSamePositionInTheOtherEnumerable()
        {
            // arrange
            var actual = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fefe",
                    LastName = "LeGrange"
                }
            };
            var expected = new List<Person>
            {
                new()
                {
                    Age = 21,
                    ChangeInPocket = .99m,
                    DateOfBirth = DateTime.Today.AddYears(-21),
                    FirstName = "Fifi",
                    LastName = "LeGrange"
                }
            };
            var comparer = new CustomEnumerableComparer<Person>();
            var expectedMessage = "Assert.Equal() Failure: Values differ\r\nExpected: A value of \"Fifi\" for property \"FirstName\"\r\nActual:   A value of \"Fefe\" for property \"FirstName\"";

            // act/assert
            var exception = Assert.Throws<EqualException>(() => comparer.Equals(expected, actual));

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}