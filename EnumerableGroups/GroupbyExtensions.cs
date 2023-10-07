namespace GroupByExtensions
{
    public static class GroupbyExtensions
    {
        /// <summary>
        /// Memory efficient alternative to GroupBy operation for IEnumerables.
        /// Assumes that the IEnumerable is "sorted" by the key selector.
        /// Sorted is used loosely to mean items with equivalent keys are guaranteed to be adjacent
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TItem">Item type</typeparam>
        /// <param name="items"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when items is not sorted by key</exception>
        public static IEnumerable<IGrouping<TKey, TItem>> GroupByKeySorted<TKey, TItem>(
            this IEnumerable<TItem> items,
            Func<TItem, TKey> keySelector) where TKey : IEquatable<TKey>
        {
            if (items.Count() == 0)
            {
                yield break;
            }

            IGrouping<TKey, TItem> group;
            IEnumerator<TItem> enumerator = items.GetEnumerator();
            ISet<TKey> keysSeen = new HashSet<TKey>();

            enumerator.MoveNext();
            bool endOfEnumerator = false;

            IEnumerable<TItem> getGroup(IEnumerator<TItem> enumerator)
            {
                TItem prev = enumerator.Current;
                TKey key = keySelector(prev);

                yield return prev;

                while (enumerator.MoveNext())
                {
                    if (!key.Equals(enumerator.Current))
                    {
                        yield break;
                    }
                }

                endOfEnumerator = true;
                yield break;
            }

            while (true)
            {
                TKey key = keySelector(enumerator.Current);
                if (keysSeen.Contains(key))
                {
                    throw new InvalidOperationException("Enumerator was not sorted by key selector, failing request");
                }

                group = new Grouping<TKey, TItem>(key, getGroup(enumerator));
                yield return group;
                keysSeen.Add(key);

                if (endOfEnumerator)
                {
                    break;
                }
            }

        }
    }
}
