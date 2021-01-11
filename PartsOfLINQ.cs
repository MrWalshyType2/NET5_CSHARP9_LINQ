using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINQ
{
    public static class PartsOfLINQ
    {
        // 1. Data Source
        internal static int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

        // 2. Query creation
        // numQuery is an IEnumerable<int>
        internal static IEnumerable<int> CreateEvenQuery()
        {
            var numQuery = from num in numbers
                           where (num % 2) == 0
                           select num;

            return numQuery;
        }
        
        // 3. Query execution
        internal static void ExecuteEvenQuery(IEnumerable<int> query)
        {
            foreach (int num in query)
            {
                Console.WriteLine("{0,1} ", num);
            }
        }
    }
}
