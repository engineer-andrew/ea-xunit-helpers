﻿using EAExtensions.PropertyInfoExtensions;
using EAExtensions.TypeExtensions;
using System.Collections;
using System.Reflection;
using Xunit.Sdk;

namespace EAXUnitHelpers.Comparison
{
    internal class Comparer
    {
        internal bool AreEnumerablesEqual<TEnumerable>(IEnumerable<TEnumerable>? expectedEnumerable,
            IEnumerable<TEnumerable>? actualEnumerable, bool includeAncestorProperties, string? propName = null, List<string>? propertiesToCheck = null)
        {
            // if both objects are null
            if (expectedEnumerable == null)
            {
                if (actualEnumerable == null)
                {
                    return true;
                }

                throw EqualException.ForMismatchedValues("A null collection", $"A collection containing {actualEnumerable.Count()} objects");
            }

            if (actualEnumerable == null)
            {
                throw EqualException.ForMismatchedValues($"A collection containing {expectedEnumerable.Count()} objects", "A null collection");
            }

            var expected = expectedEnumerable.ToList();
            var actual = actualEnumerable.ToList();

            // if the two lists don't have the same number of elements then they can't possibly be equal
            if (expected.Count != actual.Count)
            {
                if (propName != null)
                {
                    throw EqualException.ForMismatchedValues($"{expected.Count} items in {propName}",
                        $"{actual.Count} items in {propName}");
                }

                throw EqualException.ForMismatchedValues($"{expected.Count} items", $"{actual.Count} items");
            }

            // if the list of expected items has nothing in it (at this point that means the list of actual items also has nothing in it)
            // then don't even bother trying to compare values of properties
            if (expected.Count == 0)
            {
                return true;
            }

            if (typeof(TEnumerable).IsSimple())
            {
                // if what we have is something like a List<string> then we don't want to get properties, we just want to compare the items to each other
                for (var i = 0; i < expected.Count; i++)
                {
                    if (!expected[i]!.Equals(actual[i]))
                    {
                        throw EqualException.ForMismatchedValues($"A value of \"{expected[i]}\" in position {i}", $"A value of \"{actual[i]}\" in position {i}");
                    }
                }
            }
            else
            {
                // compare each object in the enumerable to the object in the expectation that is in the same index
                for (var i = 0; i < expected.Count; i++)
                {
                    AreObjectsEqual(expected[i], actual[i], includeAncestorProperties, propertiesToCheck);
                }
            }

            return true;
        }

        internal bool AreObjectsEqual<TObject>(TObject expected, TObject actual, bool includeAncestorProperties, List<string>? propertiesToCheck = null)
        {
            // check if the object to compare is a primitive type, a nullable primitive type, a string, or a decimal
            // if the object to compare is a simple (see above) type, use the built-in object value comparator (.Equals) to compare the two objects
            if (typeof(TObject).IsSimple() && !expected!.Equals(actual))
            {
                throw EqualException.ForMismatchedValues($"A value of \"{expected}\"", $"A value of \"{actual}\"");
            }

            if (typeof(TObject).IsDateTime())
            {
                var actualDateTimeValue = Convert.ToDateTime(actual);
                var expectedDateTimeValue = Convert.ToDateTime(expected);
                if (DateTime.Compare(expectedDateTimeValue, actualDateTimeValue) != 0)
                {
                    throw EqualException.ForMismatchedValues($"A value of \"{expectedDateTimeValue}\"",
                        $"A value of \"{actualDateTimeValue}\"");
                }

                return true;
            }

            var props = includeAncestorProperties
                ? typeof(TObject).GetProperties()
                : typeof(TObject).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (propertiesToCheck != null && propertiesToCheck.Count > 0)
            {
                props = props.Where(p => propertiesToCheck.Contains(p.Name)).ToArray();
            }

            // iterate through the properties on the complex object
            foreach (var prop in props)
            {
                // get the value of this property from each object for comparison
                var expectedValue = prop.GetValue(expected, null);
                var actualValue = prop.GetValue(actual, null);

                // if both objects are null
                if (expectedValue == null)
                {
                    if (actualValue == null)
                    {
                        continue;
                    }

                    throw EqualException.ForMismatchedValues($"A value of null for property \"{prop.Name}\"",
                        $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                }

                if (actualValue == null)
                {
                    throw EqualException.ForMismatchedValues($"A value of \"{expectedValue}\" for property \"{prop.Name}\"", $"A value of null for property \"{prop.Name}\"");
                }

                if (prop.PropertyType.IsSimple())
                {
                    if (!expectedValue.Equals(actualValue))
                    {
                        throw EqualException.ForMismatchedValues($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                            $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                    }
                }
                else if (prop.PropertyType.IsDateTime())
                {
                    var actualDateTimeValue = (DateTime)actualValue;
                    var expectedDateTimeValue = (DateTime)expectedValue;
                    if (DateTime.Compare(expectedDateTimeValue, actualDateTimeValue) != 0)
                    {
                        throw EqualException.ForMismatchedValues($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                            $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                    }
                }
                else if (prop.PropertyType.IsDateTimeOffset())
                {
                    var actualDateTimeOffsetValue = (DateTimeOffset)actualValue;
                    var expectedDateTimeOffsetValue = (DateTimeOffset)expectedValue;
                    if (DateTimeOffset.Compare(expectedDateTimeOffsetValue, actualDateTimeOffsetValue) != 0)
                    {
                        throw EqualException.ForMismatchedValues($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                            $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                    }
                }
                else if (prop.IsNonStringEnumerable())
                {
                    // get the type of the object in the enumerable
                    var genericArguments = prop.PropertyType.GetGenericArguments();
                    if (genericArguments.Length > 0)
                    {
                        var objectType = genericArguments[0];
                        // create a List<objectType>
                        var genericListType = typeof(List<>).MakeGenericType(objectType);
                        // instantiate lists with the values in the enumerables;
                        var expectedList = Activator.CreateInstance(genericListType, expectedValue) as IList;
                        var actualList = Activator.CreateInstance(genericListType, actualValue) as IList;
                        AreEnumerablesEqual(expectedList as dynamic, actualList as dynamic, includeAncestorProperties, prop.Name);
                    }
                }
                else
                {
                    AreObjectsEqual((dynamic)expectedValue, (dynamic)actualValue, includeAncestorProperties, propertiesToCheck);
                }
            }

            return true;
        }
    }
}