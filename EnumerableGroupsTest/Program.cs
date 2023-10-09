using GroupByExtensions;

namespace GroupByExtensionsTest
{
    public static class Program
    {
        private static int numGroups = 100000;
        private static int sizeOfGroup = 100;

        static void Main()
        {
            testMemoryEfficient();
            testCurrent();
        }

        static void testMemoryEfficient()
        {
            var records = GetRecords();

            var groups = records.GroupByKeySorted(r => r.Key);

            AccessRecords(groups);
        }

        static void testCurrent()
        {
            var records = GetRecords();

            var groups = records.GroupBy(r => r.Key);

            AccessRecords(groups);
        }

        private static IEnumerable<Record> GetRecords()
        {
            for(int i = 0; i < numGroups; i++)
            {
                for(int j = 0; j < sizeOfGroup; j++)
                {
                    yield return new Record(i, j);
                }
            }
        }

        /// <summary>
        /// This could be any method that only needs a subset of records in memory at a time.
        /// Such as writing to a file, or performing some operation only at group level.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="groups"></param>
        private static void AccessRecords<TKey>(IEnumerable<IGrouping<TKey, Record>> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine(group.Select(r => r.Value).Sum());
            }
        }
    }
}
