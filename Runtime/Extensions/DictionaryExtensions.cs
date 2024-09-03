using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Utils.Dictionary
{
  // Class that defines extension methods for dictionaries
  public static class DictionaryExtensions
  {
    #region Selecting keys and values of dictionaries
    // Project each item of enumerable of key-value pairs into its key
    public static IEnumerable<TKey> SelectKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));

      return enumerable.Select(e => e.Key);
    }

    // Project each item of enumerable of key-value pairs into its value
    public static IEnumerable<TValue> SelectValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));

      return enumerable.Select(e => e.Value);
    }


    // Project each key of an enumerable of key-value pairs into a new form
    public static IEnumerable<KeyValuePair<TKeyResult, TValue>> SelectOnKey<TKey, TValue, TKeyResult>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TKey, TKeyResult> keySelector)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));
      if (keySelector == null)
        throw new ArgumentNullException(nameof(keySelector));

      return enumerable.ToDictionary(e => keySelector(e.Key), e => e.Value);
    }

    // Project each value of an enumerable of key-value pairs into a new form
    public static IEnumerable<KeyValuePair<TKey, TValueResult>> SelectOnValue<TKey, TValue, TValueResult>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValueResult> valueSelector)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));
      if (valueSelector == null)
        throw new ArgumentNullException(nameof(valueSelector));

      return enumerable.ToDictionary(e => e.Key, e => valueSelector(e.Value));
    }
    #endregion

    #region Converting enumerables to dictionaries
    // Convert an enumerable of key-value pairs to a dictionary
    public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> enumerable)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));

      return enumerable.ToDictionary(e => e.Key, e => e.Value);
    }
    #endregion

    #region Merging dictionaries


    // Merge two dictionaries into one
    public static Dictionary<TKey, TElement> Merge<TKey, TElement>(this IReadOnlyDictionary<TKey, TElement> dictionary, IReadOnlyDictionary<TKey, TElement> other, DictionaryMergeStrategy<TKey, TElement> strategy)
    {
      if (dictionary == null)
        throw new ArgumentNullException(nameof(dictionary));
      if (other == null)
        throw new ArgumentNullException(nameof(other));
      if (strategy == null)
        throw new ArgumentNullException(nameof(strategy));
        
      return dictionary.Concat(other)
        .ToLookup(e => e.Key, e => e.Value)
        .ToDictionary(g => g.Key, g => strategy(g));
    }
    #endregion
  }
}