namespace EAXUnitHelpers.Comparison
{
    public class CustomObjectComparer<T> : IEqualityComparer<T>
    {
        private readonly bool _includeAncestorProperties;
        private readonly List<string> _props;

        public CustomObjectComparer() : this(false)
        {
        }

        public CustomObjectComparer(bool includeAncestorProperties)
        {
            _includeAncestorProperties = includeAncestorProperties;
            _props = new List<string>();
        }

        public CustomObjectComparer(List<string> props)
        {
            _props = props;
        }

        public CustomObjectComparer(bool includeAncestorProperties, List<string> props)
        {
            _includeAncestorProperties = includeAncestorProperties;
            _props = props;
        }

        public bool Equals(T expected, T actual)
        {
            var utilities = new Comparer();
            return utilities.AreObjectsEqual(expected, actual, _includeAncestorProperties, _props);
        }

        public int GetHashCode(T parameterValue)
        {
            return Tuple.Create(parameterValue).GetHashCode();
        }
    }
}