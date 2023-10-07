using GroupByExtensions;

namespace GroupByExtensionsTest
{
    public static class Program
    {
        static void Main()
        {
            IEnumerable<Record> records = new List<Record>
            {
                new Record("1", "hi"),
                new Record("1", "hi2"),
                new Record("2", "hi"),
                new Record("1", "fail")
            };

            IEnumerable<IGrouping<String, Record>> groups = records.GroupByKeySorted(r => r.Key);

            //We only load each record in memory individually here,
            //while still being able to perform group operations.

            //This is super powerful for performing complicated
            //query operations while being memory efficient
            foreach(var group in groups)
            {
                Console.WriteLine($"Key: {group.Key}");
                foreach(var record in group)
                {
                    Console.WriteLine(record);
                }
            }
        }
    }
}
