# LINQ (Language-Integrated Query)[https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/]
LINQ is a set of technologies based on the integration of query capabilities directly into the C# language.

LINQ queries are a first-class language construct like classes, methods and events. Queries are wrote against
strongly typed collections by using language keywords and familiar operators.

LINQ provides a consistant query experience for:
- Objects (LINQ to Objects)
- Relational databases (LINQ to SQL)
- XML (LINQ to XML)

A 'query expression' is written using a declarative 'query syntax' which can perform filtering, ordering, and grouping 
operations on data sources. The same basic query expression patterns can be used to query and transform data in 
SQL databases, ADO.NET Datasets, XML documents and streams, and .NET collections.

You can write LINQ queries in C# for SQL Server databases, XML documents, ADO.NET Datasets, and any collection of objects 
that supports IEnumerable or the generic IEnumerable<T> interface.

## Three parts of a Query Operation
1. Obtain the data source
2. Create the query
3. Execute the query

See PartsOfLINQ.cs for example

### The Data Source
The data source must support the `IEnumerable` or generic `IEnumerable<T>` interfaces to be queryable with LINQ. An interface derived 
from `IEnumerable<T>` such as the generic `IQueryable<T>` is known as a 'queryable type'.

#### XElement
```
// Create a data source from an XML document.
// using System.Xml.Linq;
XElement contacts = XElement.Load(@"c:\myContactList.xml");
```

#### LINQ to SQL
First, an object-relational mapping is either manually created, or created with the 'LINQ to SQL Tools' in Visual Studio. The queries
are written against objects and evaluated at runtime.
```
Northwnd db = new Northwnd(@"c:\northwnd.mdf");  
  
// Query for customers in London.  
IQueryable<Customer> custQuery =  
    from cust in db.Customers  
    where cust.City == "London"  
    select cust;
```

### The Query
The query specifies what information is to be retrieved from a data source/s. Queries can optionally specify how the
information should be sorted, grouped, and shaped before it is returned. Queries are stored in a query variable and initialised
with a query expression.

#### Basic Syntax
The `from`, `where`, and `select` clauses are used in a similar fashion to SQL, except in reverse.

- `from` specifies the data source
- `where` applies a filter to the data
- `select` specifies the type of the returned elements.

### Query Execution
#### Deferred Execution
Query variables only store query commands so the actual execution is deferred until the query variable
is iterated over in a `foreach` statement.

```
//  Query execution.
foreach (int num in numQuery)
{
    Console.Write("{0,1} ", num);
}
```

Query variables never store the query results themselves and so can be executed as frequently as wanted.

#### Forced Execution
A query that performs an aggregation function over a range of source elements must iterate over those sources elements, 
this is not possible with deferred execution. The `Count`, `Max`, `Average` and `First` queries are examples of forced execution
as they execute a `foreach` statement without an explicit call.

Calling the `ToList` or `ToArray` methods will force the immediate execution of any query and caching of its results.
```
var evenNumQuery =
    from num in numbers
    where (num % 2) == 0
    select num;

int evenNumCount = evenNumQuery.Count();

List<int> numQuery2 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToList();

// or like this:
// numQuery3 is still an int[]

var numQuery3 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToArray();
```

## LINQ & Generic Types
LINQ queries are based on generic types, introduced in .NET 2.0.

### Two Basic Generic Concepts
- A generic collection class will hold objects of the specified type `T`. A generic `List<T>` can only hold strings if the type specified is a string: `List<string>`.
- `IEnumerable<T>` allows generic collection classes to be enumerated with the `foreach` statement.

### IEnumerable<T> variables in LINQ Queries
LINQ query variables are typed as IEnumerable<T> or a derived type such as IQueryable<T>. The example below will retrieve 0 or more dogs for example.

```
IEnumerable<Dog> getOldDogsQuery =
    from dog in dogs
    where dog.age >= 10
    select dog;

foreach (Dog dog in getOldDogsQuery)
{
    Console.WriteLine(dog.LastName + ", " + dog.FirstName);
}
```

## Basic LINQ Query Operations
### Obtaining a Data Source
The first step in a LINQ query is to specify the data source. The `from` clause introduces the data source and the range variable:
```
IEnumerable<T> = from <range> in <collection>
                 select <range>;
```

The 'range' variable is like the iteration variable in a `foreach` loop, except that no actual iteration occurs in a query expression.

Introduce additional range variables with the `let` clause:
```
class LetSample1
{
    static void Main()
    {
        string[] strings =
        {
            "A penny saved is a penny earned.",
            "The early bird catches the worm.",
            "The pen is mightier than the sword."
        };

        // Split the sentence into an array of words
        // and select those whose first letter is a vowel.
        var earlyBirdQuery =
            from sentence in strings
            let words = sentence.Split(' ')
            from word in words
            let w = word.ToLower()
            where w[0] == 'a' || w[0] == 'e'
                || w[0] == 'i' || w[0] == 'o'
                || w[0] == 'u'
            select word;

        // Execute the query.
        foreach (var v in earlyBirdQuery)
        {
            Console.WriteLine("\"{0}\" starts with a vowel", v);
        }

        // Keep the console window open in debug mode.
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
/* Output:
    "A" starts with a vowel
    "is" starts with a vowel
    "a" starts with a vowel
    "earned." starts with a vowel
    "early" starts with a vowel
    "is" starts with a vowel
*/
```

### Filtering
The most common query operation is applying a predicate (boolean expression) as a filter. The filter will then make the query only return
elements for which the expression is true. The result is produced using the `where` clause.
```
var someQuery = from <range> in <collection>
                where <range>.<Property/Field> == <SomeValue>
                select <range>;
```

The logical `AND` and `OR` operators can be used to apply as many filter expressions as necessary.
```
where <range>.<Property/Field> == <SomeValue> && <range>.<Property/Field> != <SomeValue>
```

### Ordering
The `orderby` clause causes elements in a returned sequence to be sorted according to the default comparer for the type being sorted.

The following query sorts based on the `Name` property, which as a string is sorted by default from A to Z:
```
var queryLondonCustomers3 =
    from cust in customers
    where cust.City == "London"
    orderby cust.Name ascending
    select cust;
```

To reverse the results, use the `descending` keyword in the `orderby` clause.

### Grouping
The `group` clause enables grouping results based a specified key.
```
// queryCustomersByCity is an IEnumerable<IGrouping<string, Customer>>
  var queryCustomersByCity =
      from cust in customers
      group cust by cust.City;

  // customerGroup is an IGrouping<string, Customer>
  foreach (var customerGroup in queryCustomersByCity)
  {
      Console.WriteLine(customerGroup.Key);
      foreach (Customer customer in customerGroup)
      {
          Console.WriteLine("    {0}", customer.Name);
      }
  }
```

A query that ends with a `group` clause takes the form of a list of lists where each element in the list is
an object with a `Key` member and a list of elements grouped under the key. Iterating over the query produces a sequence of groups (use a nested `foreach`).

Sometimes, the results of a group operation need to be referred to. The `into` keyword can be used to create a queryable identifier:
```
// custQuery is an IEnumerable<IGrouping<string, Customer>>
var custQuery =
    from cust in customers
    group cust by cust.City into custGroup
    where custGroup.Count() > 2
    orderby custGroup.Key
    select custGroup;
```

### Joining
A `join` clause creates associations between sequences not explicitly modelled in data sources. The `join` clause
will always work against object collections instead of DB tables directly.
```
var innerJoinQuery =
    from cust in customers
    join dist in distributors on cust.City equals dist.City
    select new { CustomerName = cust.Name, DistributorName = dist.Name };
```

The `join` cluase is not used as often as joins in SQL as foreign keys are represented in the object model as properties
that hold a collection of items. Rather than performing a join, just the dot notation:
```
// Customer has a collection of Order items
from order in Customer.Orders
```