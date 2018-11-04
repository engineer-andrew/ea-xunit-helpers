using EAExtensions.PropertyInfoExtensions;
using EAExtensions.TypeExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace EAXUnitHelpers.Comparison
{
    internal class Comparer
    {
        internal bool AreEnumerablesEqual<TEnumerable>(IEnumerable<TEnumerable> expectedEnumerable,
            IEnumerable<TEnumerable> actualEnumerable)
        {
            var expected = expectedEnumerable.ToList();
            var actual = actualEnumerable.ToList();

            // if the two lists don't have the same number of elements then they can't possibly be equal
            if (expected.Count != actual.Count)
            {
                throw new EqualException($"{expected.Count} items", $"{actual.Count} items");
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
                    if (!expected[i].Equals(actual[i]))
                    {
                        throw new EqualException($"A value of \"{expected[i]}\"", $"A value of \"{actual[i]}\"");
                    }
                }
            }
            else
            {
                // we're dealing with an IEnumerable of some complex (probably user-defined) type so get its properties
                var props = typeof(TEnumerable).GetProperties(
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                // go through each property on each item in each list and compare the values of the properties to each other
                for (var i = 0; i < expected.Count; i++)
                {
                    foreach (var prop in props)
                    {
                        var expectedValue = prop.GetValue(expected[i], null);
                        var actualValue = prop.GetValue(actual[i], null);

                        if (expectedValue == null)
                        {
                            if (actualValue == null)
                            {
                                continue;
                            }

                            throw new EqualException($"A value of null for property \"{prop.Name}\"",
                                $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                        }

                        if (actualValue == null)
                        {
                            throw new EqualException($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                                $"A value of null for property \"{prop.Name}\"");
                        }

                        // if the property is a simple type, just compare the two values and throw an exception if they're different
                        if (prop.PropertyType.IsSimple())
                        {
                            if (!expectedValue.Equals(actualValue))
                            {
                                throw new EqualException($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                                    $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                            }
                        }
                        // if the property is itself a non-string Enumerable, then recursively check its objects for equality
                        else if (prop.IsNonStringEnumerable())
                        {
                            // get the type of the object in the enumerable
                            var objectType = prop.PropertyType.GetGenericArguments()[0];
                            // create a List<objectType>
                            var genericListType = typeof(List<>).MakeGenericType(objectType);
                            // instantiate lists with the values in the enumerables;
                            var expectedList = (IList)Activator.CreateInstance(genericListType, expectedValue);
                            var actualList = (IList)Activator.CreateInstance(genericListType, actualValue);
                            AreEnumerablesEqual((dynamic)expectedList, (dynamic)actualList);
                        }
                        else
                        {
                            // the property is a complex (probably user-defined) type so we need to iterate its own properties and compare the values
                            AreObjectsEqual((dynamic)expectedValue, (dynamic)actualValue);
                        }
                    }
                }
            }

            return true;
        }

        internal bool AreObjectsEqual<TObject>(TObject expected, TObject actual)
        {
            if (typeof(TObject).IsSimple() && !expected.Equals(actual))
            {
                throw new EqualException($"A value of \"{expected}\"", $"A value of \"{actual}\"");
            }

            var props = typeof(TObject).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                      BindingFlags.Instance);

            foreach (var prop in props)
            {
                var expectedValue = prop.GetValue(expected, null);
                var actualValue = prop.GetValue(actual, null);

                if (expectedValue == null)
                {
                    if (actualValue == null)
                    {
                        continue;
                    }

                    throw new EqualException($"A value of null for property \"{prop.Name}\"",
                        $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                }

                if (actualValue == null)
                {
                    throw new EqualException($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                        $"A value of null for property \"{prop.Name}\"");
                }

                if (prop.IsNonStringEnumerable())
                {
                    // get the type of the object in the enumerable
                    var objectType = prop.PropertyType.GetGenericArguments()[0];
                    // create a List<objectType>
                    var genericListType = typeof(List<>).MakeGenericType(objectType);
                    // instantiate lists with the values in the enumerables;
                    var expectedList = (IList)Activator.CreateInstance(genericListType, expectedValue);
                    var actualList = (IList)Activator.CreateInstance(genericListType, actualValue);
                    AreEnumerablesEqual((dynamic)expectedList, (dynamic)actualList);
                }
                else if (!expectedValue.Equals(actualValue))
                {
                    throw new EqualException($"A value of \"{expectedValue}\" for property \"{prop.Name}\"",
                        $"A value of \"{actualValue}\" for property \"{prop.Name}\"");
                }
            }

            return true;
        }
    }
}
