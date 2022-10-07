namespace EAXUnitHelpers.Comparison
{
    public class CustomEnumerableComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly bool _includeAncestorProperties;
        private readonly List<string> _props;

        public CustomEnumerableComparer() : this(false)
        {
        }

        public CustomEnumerableComparer(bool includeAncestorProperties)
        {
            _includeAncestorProperties = includeAncestorProperties;
            _props = new List<string>();
        }

        public CustomEnumerableComparer(List<string> props)
        {
            _props = props;
        }

        public CustomEnumerableComparer(bool includeAncestorProperties, List<string> props)
        {
            _includeAncestorProperties = includeAncestorProperties;
            _props = props;
        }

        public bool Equals(IEnumerable<T> expectedEnumerable, IEnumerable<T> actualEnumerable)
        {
            var utilities = new Comparer();
            return utilities.AreEnumerablesEqual(expectedEnumerable, actualEnumerable, _includeAncestorProperties, propertiesToCheck: _props);
        }

        public int GetHashCode(IEnumerable<T> parameterValue)
        {
            return Tuple.Create(parameterValue).GetHashCode();
        }
    }
}