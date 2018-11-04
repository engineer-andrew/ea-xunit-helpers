# EAXUnitHelpers.Comparison

This package contains custom comparators for objects and enumerables. In order to use this package with XUnit, add the new CustomObjectComparer<T> or CustomEnumerableComparer<T> to your assertion statement (```Assert.Equal(expected, actual, new CustomObjectComparer<MySpecialObject>())```).

The contents of this package are based on an answer to a question on this topic on StackOverflow, which can be found [here](https://stackoverflow.com/questions/11135337/xunit-assertion-for-checking-equality-of-objects/49825057).