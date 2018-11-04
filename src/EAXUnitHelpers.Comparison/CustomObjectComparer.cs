using System;
using System.Collections.Generic;

namespace EAXUnitHelpers.Comparison
{
    public class CustomObjectComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T expected, T actual)
        {
            var utilities = new Comparer();
            return utilities.AreObjectsEqual(expected, actual);
        }

        public int GetHashCode(T parameterValue)
        {
            return Tuple.Create(parameterValue).GetHashCode();
        }
    }
}