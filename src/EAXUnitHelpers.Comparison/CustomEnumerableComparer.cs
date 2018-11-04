using System;
using System.Collections.Generic;

namespace EAXUnitHelpers.Comparison
{
    public class CustomEnumerableComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> expectedEnumerable, IEnumerable<T> actualEnumerable)
        {
            var utilities = new Comparer();
            return utilities.AreEnumerablesEqual(expectedEnumerable, actualEnumerable);
        }

        public int GetHashCode(IEnumerable<T> parameterValue)
        {
            return Tuple.Create(parameterValue).GetHashCode();
        }
    }
}