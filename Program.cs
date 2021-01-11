using System;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartsOfLINQ.cs
            var query = PartsOfLINQ.CreateEvenQuery();
            PartsOfLINQ.ExecuteEvenQuery(query);
        }
    }
}
