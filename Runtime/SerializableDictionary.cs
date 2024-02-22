using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audune.Utils.Dictionary
{
  // Class that defines a serializable dictionary
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    // Class that defines an entry in a serializable dictionary
    [Serializable]
    public struct Entry
    {
      // The key and value of the entry
      public TKey key;
      public TValue value;


      // Constructor
      public Entry(TKey key, TValue value)
      {
        this.key = key;
        this.value = value;
      }
    }


    // The list of entries of the dictionary
    [SerializeField]
    private List<Entry> _entries = new List<Entry>();


    // The dictionary that backs the entries in the dictionary
    private Dictionary<TKey, TValue> _dictionary;


    // Constructor
    public SerializableDictionary()
    {
      _dictionary = new Dictionary<TKey, TValue>();
    }

    // Constructor from another dictionary
    public SerializableDictionary(IDictionary<TKey, TValue> dictionary) 
    {
      _dictionary = new Dictionary<TKey, TValue>(dictionary);
    }


    // Return if the dictionary contains duplicate keys
    private bool ContainsDuplicatedKeys()
    {
      return _entries.GroupBy(e => e.key).Where(g => g.Count() > 1).Select(g => g.Key).Any();
    }


    #region Dictionary implementation
    // Return a value in the dictionary for the specified key
    public TValue this[TKey key] {
      get => ((IDictionary<TKey, TValue>)_dictionary)[key];
      set => ((IDictionary<TKey, TValue>)_dictionary)[key] = value;
    }

    // Return the keys of the dictionary
    public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)_dictionary).Keys;

    // Return the values of the dictionary
    public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_dictionary).Values;

    // Return the number of entries of the dictionary
    public int Count => ((IDictionary<TKey, TValue>)_dictionary).Count;

    // Return if the dictionary is read-only
    public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dictionary).IsReadOnly;

    // Return the keys of the dictionary as an enumerable
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)_dictionary).Keys;

    // Return the values of the dictionary as an enumerable
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)_dictionary).Values;


    // Return if the dictionary contains the specified key
    public bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>)_dictionary).ContainsKey(key);

    // Return if the dictionary contains the specified key and store its value
    public bool TryGetValue(TKey key, out TValue value) => ((IDictionary<TKey, TValue>)_dictionary).TryGetValue(key, out value);

    // Add the specified entry to the dictionary
    public void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>)_dictionary).Add(key, value);

    // Remove the entry with the specified key from the dictionary
    public bool Remove(TKey key) => ((IDictionary<TKey, TValue>)_dictionary).Remove(key);

    // Clear the dictionary
    public void Clear() => ((IDictionary<TKey, TValue>)_dictionary).Clear();

    // Return a generic enumerator for the dictionary
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();

    // Return an enumerator for the dictionary
    IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();


    // Return if the dictionary contains the specified key-value pair
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Contains(item);

    // Add the specified key-value pair to the dictionary
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Add(item);

    // Remove the specified key-value pair from the dictionary
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Remove(item);

    // Copy the dictionary to the specified array
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
    #endregion

    #region Serialization callbacks
    // Callback received before Unity serializes the dictionary
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
      if (!ContainsDuplicatedKeys())
      {
        _entries.Clear();
        foreach (var e in this)
          _entries.Add(new Entry(e.Key, e.Value));
      }
    }

    // Callback received after Unity deserializes the dictionary
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      if (!ContainsDuplicatedKeys())
      {
        _dictionary.Clear();
        foreach (var e in _entries)
          _dictionary.Add(e.key, e.value);
      }
    }
    #endregion
  }
}