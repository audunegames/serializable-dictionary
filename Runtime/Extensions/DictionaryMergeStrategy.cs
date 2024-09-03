using System.Linq;

namespace Audune.Utils.Dictionary
{
  // Delegate that defines a strategy to merge dictionaries
  public delegate TElement DictionaryMergeStrategy<TKey, TElement>(IGrouping<TKey, TElement> values);


  // Class that defines common merge strategies
  public static class DictionaryMergeStrategy
  {
    // Return a merge strategy that selects the first element
    public static DictionaryMergeStrategy<TKey, TElement> First<TKey, TElement>()
    {
      return g => g.First();
    }

    // Return a merge strategy that selects the last element
    public static DictionaryMergeStrategy<TKey, TElement> Last<TKey, TElement>()
    {
      return g => g.Last();
    }
  }
}