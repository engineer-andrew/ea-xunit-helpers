# EAXUnitHelpers.Comparison

This package contains custom comparators for objects and enumerables. The following uses are supported:

  * ```Assert.Equal(expected, actual, new CustomObjectComparer<MySpecialObject>())```
  * ```Assert.Equal(expected, actual, new CustomEnumerableComparer<MySpecialObject>())```
  * ```Assert.Equal(expected, actual, new CustomObjectComparer<MySpecialObject>(true))```
  * ```Assert.Equal(expected, actual, new CustomEnumerableComparer<MySpecialObject>(true))```
  * ```Assert.Equal(expected, actual, new CustomObjectComparer<MySpecialObject>(new List<string> {"Name", "City"}))```
  * ```Assert.Equal(expected, actual, new CustomEnumerableComparer<MySpecialObject>(new List<string> {"Name", "City"}))```
  * ```Assert.Equal(expected, actual, new CustomObjectComparer<MySpecialObject>(true, new List<string> {"Name", "City"}))```
  * ```Assert.Equal(expected, actual, new CustomEnumerableComparer<MySpecialObject>(true, new List<string> {"Name", "City"}))```

CustomObjectComparer should be used when two objects need to be compared (which should be obvious) and CustomEnumerableComparer should be used when two enumerables (IEnumerable, List, array, etc.) should be compared (which, again, should be obvious).

Passing true to the constructor of either CustomObjectComparer or CustomEnumerableComparer will result in the comparison checking inherited properties as well.

Passing a list of properties will restrict the comparison of the objects to those properties passed. This is case sensitive so if you want to check the "Name" then you must pass "Name" and not "name".

The contents of this package are based on an answer to a question on this topic on StackOverflow, which can be found [here](https://stackoverflow.com/questions/11135337/xunit-assertion-for-checking-equality-of-objects/49825057).