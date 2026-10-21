using LinqMastery.Data;
using LinqMastery.Models;
using System.Linq;

namespace LinqMastery;

public class Program
{
    public static void Main()
    {
        Dele dele = new Dele();
        dele.Run();
        Console.ReadKey();

        Console.WriteLine("LINQ Mastery Practice Project");
        Console.WriteLine("Uncomment one exercise method at a time in Main().");

        Exercise01_Filtering();
        Exercise02_Projection();
        //Exercise03_SelectMany();
        //Exercise04_Sorting();
        //Exercise05_ElementOperators();
        //Exercise06_Aggregation();
        //Exercise07_AnyAll();
        //Exercise08_Grouping();
        //Exercise09_SetOperations();
        //Exercise10_InnerJoin();
        //Exercise11_LeftJoin();
        //Exercise12_GroupJoin();
        //Exercise13_Partitioning();
        //Exercise14_QuantifiersAndMembership();
        //Exercise15_StringAndNumericQueries();
        // Exercise16_NestedQueries();
        // Exercise17_CombinedQueries();
        // Exercise18_DictionaryAndLookup();
        // Exercise19_JoinOrdersAndCustomers();
        // Exercise20_Reports();
    }

    // RULE: For every task, implement both styles where supported.
    // Use these labels to keep your work organised:
    // METHOD SYNTAX PRACTICE
    // QUERY SYNTAX PRACTICE

    static void Exercise01_Filtering()
    {
        var products = StoreData.Products;

        // Task 1: Find products priced above 5,000.
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Where(p => p.Price > 5000);

        //Console.WriteLine("\nMethod Syntax:");
        //foreach (var product in methodResult)
        //{
        //    Console.WriteLine($"{product.ProductName} - {product.Price}");
        //}

        // QUERY SYNTAX PRACTICE
        //var queryresult = from product in products
        //                  where product.Price > 5000
        //                  select product;

        // Console.WriteLine("Query Syntax");
        // foreach(var product in queryresult)
        // {
        //     Console.WriteLine($"{product.ProductName} - {product.Price}");
        // }
        // Console.ReadKey();

        // Task 2: Find Electronics products that are in stock.
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Where(p => p.Category == "Electronics" && p.Stock > 0);
        //foreach (var product in methodResult)
        //{
        //    Console.WriteLine($"{product.Category}- {product.ProductName}");
        //}
        //Console.ReadKey();


        // QUERY SYNTAX PRACTICE
        //var queryResult = from Product in products
        //                  where Product.Category == "Electronics" && Product.Stock > 0
        //                  select Product;
        //foreach (var product in queryResult)
        //{
        //    Console.WriteLine($"{product.Category}-{product.ProductName}");
        //}
        //Console.ReadKey();

        // Task 3: Find products priced between 1,000 and 20,000.
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Where(p => p.Price >= 1000 && p.Price =< 20000);
        //foreach (var product in methodResult)
        //{
        //    Console.WriteLine($"{product.ProductName} - {product.Price}");
        //}
        //Console.ReadKey();


        //// QUERY SYNTAX PRACTICE
        //var queryResult = from product in products
        //                  where product.Price >= 1000 && product.Price =< 20000
        //                  select product;
        //foreach (var product in queryResult)
        //{
        //    Console.WriteLine($"{product.ProductName} - {product.Price}");
        //}
        //Console.ReadKey();

        // Task 4: Find products whose names contain "Office".
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Where(p => p.ProductName.Contains("Office"));
        //foreach (var product in methodResult)
        //{
        //    Console.WriteLine(product.ProductName);
        //}
        //Console.ReadKey();

        //// QUERY SYNTAX PRACTICE
        //var queryResult = from product in products
        //                  where product.ProductName.Contains("Office")
        //                  select product;
        //foreach (var product in queryResult)
        //{
        //    Console.WriteLine(product.ProductName);
        //}
        //Console.ReadKey();
    }

    static void Exercise02_Projection()
    {
        var products = StoreData.Products;

        // Task 1: Select only product names.
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Select(p => p.ProductName);
        //foreach (var name in methodResult)
        //{
        //    Console.WriteLine(name);
        //}
        //Console.ReadKey();

        //// QUERY SYNTAX PRACTICE
        //var queryResult = from product in products
        //                  select product.ProductName;
        //foreach (var name in queryResult)
        //{
        //    Console.WriteLine(name);
        //}
        //Console.ReadKey();


        // Task 2: Return ProductName and Price using an anonymous type.
        // METHOD SYNTAX PRACTICE
        //var methodResult = products.Select(p => new { p.ProductName, p.Price });
        //foreach (var product in methodResult)
        //{
        //    Console.WriteLine($"{product.ProductName} - {product.Price}");
        //}
        //Console.ReadKey();


        //// QUERY SYNTAX PRACTICE
        //var queryResult = from product in products
        //                  select new
        //                  {
        //                      product.ProductName,
        //                      product.Price
        //                  };
        //foreach (var product in queryResult)
        //{
        //    Console.WriteLine($"{product.ProductName} - {product.Price}");
        //}
        //Console.ReadKey();


        // Task 3: Return ProductName, Price, and PriceWithTax (18%).
        // METHOD SYNTAX PRACTICE
        

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    //static void Exercise03_SelectMany()
    //{
    //    var batches = StoreData.TrainingBatches;

    //    // Task 1: Flatten all training batches into one sequence.

    //    // METHOD SYNTAX
    //    var result1 = batches.SelectMany(b => b.Trainees);
    //    foreach (var item in result1)
    //        Console.WriteLine(item);

    //    // QUERY SYNTAX
    //    var result2 = from b in batches
    //                  from t in b.Trainees
    //                  select t;

    //    foreach (var item in result2)
    //        Console.WriteLine(item);


    //    // Task 2: Get every product tag as one flat sequence.

    //    // METHOD SYNTAX
    //    var result3 = StoreData.Products.SelectMany(p => p.Tags);
    //    foreach (var tag in result3)
    //        Console.WriteLine(tag);

    //    // QUERY SYNTAX
    //    var result4 = from p in StoreData.Products
    //                  from tag in p.Tags
    //                  select tag;

    //    foreach (var tag in result4)
    //        Console.WriteLine(tag);
    //}

    //static void Exercise04_Sorting()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: Sort by price ascending.

    //    // METHOD SYNTAX
    //    var result1 = products.OrderBy(p => p.Price);
    //    foreach (var p in result1)
    //        Console.WriteLine(p.Price);

    //    // QUERY SYNTAX
    //    var result2 = from p in products
    //                  orderby p.Price
    //                  select p;

    //    foreach (var p in result2)
    //        Console.WriteLine(p.Price);


    //    // Task 2: Sort by price descending.

    //    // METHOD SYNTAX
    //    var result3 = products.OrderByDescending(p => p.Price);
    //    foreach (var p in result3)
    //        Console.WriteLine(p.Price);

    //    // QUERY SYNTAX
    //    var result4 = from p in products
    //                  orderby p.Price descending
    //                  select p;

    //    foreach (var p in result4)
    //        Console.WriteLine(p.Price);


    //    // Task 3: Category ascending, Price descending.

    //    // METHOD SYNTAX
    //    var result5 = products
    //        .OrderBy(p => p.Category)
    //        .ThenByDescending(p => p.Price);

    //    foreach (var p in result5)
    //        Console.WriteLine(p.Category + " " + p.Price);

    //    // QUERY SYNTAX
    //    var result6 = from p in products
    //                  orderby p.Category, p.Price descending
    //                  select p;

    //    foreach (var p in result6)
    //        Console.WriteLine(p.Category + " " + p.Price);
    //}

    //static void Exercise05_ElementOperators()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: First product priced above 10000.

    //    // METHOD SYNTAX
    //    var result1 = products.First(p => p.Price > 10000);
    //    Console.WriteLine(result1.ProductId);

    //    // QUERY SYNTAX
    //    var query1 = from p in products
    //                 where p.Price > 10000
    //                 select p;

    //    var result2 = query1.First();
    //    Console.WriteLine(result2.ProductId);


    //    // Task 2: Search for ProductId 999 safely.

    //    // METHOD SYNTAX
    //    var result3 = products.FirstOrDefault(p => p.ProductId == 999);

    //    if (result3 != null)
    //        Console.WriteLine(result3.ProductId);
    //    else
    //        Console.WriteLine("Product not found");


    //    // QUERY SYNTAX
    //    var query2 = from p in products
    //                 where p.ProductId == 999
    //                 select p;

    //    var result4 = query2.FirstOrDefault();

    //    if (result4 != null)
    //        Console.WriteLine(result4.ProductId);
    //    else
    //        Console.WriteLine("Product not found");


    //    // Task 3: Find ProductId 3 using Single().

    //    // METHOD SYNTAX
    //    var result5 = products.Single(p => p.ProductId == 3);
    //    Console.WriteLine(result5.ProductId);

    //    // QUERY SYNTAX
    //    var query3 = from p in products
    //                 where p.ProductId == 3
    //                 select p;

    //    var result6 = query3.Single();
    //    Console.WriteLine(result6.ProductId);
    //}

    //static void Exercise06_Aggregation()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: Count all products.

    //    // METHOD SYNTAX
    //    var result1 = products.Count();
    //    Console.WriteLine(result1);

    //    // QUERY SYNTAX
    //    var query1 = from p in products
    //                 select p;

    //    Console.WriteLine(query1.Count());


    //    // Task 2: Calculate total stock.

    //    // METHOD SYNTAX
    //    var result2 = products.Sum(p => p.Stock);
    //    Console.WriteLine(result2);

    //    // QUERY SYNTAX
    //    var query2 = from p in products
    //                 select p.Stock;

    //    Console.WriteLine(query2.Sum());


    //    // Task 3: Average, minimum and maximum price.

    //    // METHOD SYNTAX
    //    Console.WriteLine(products.Average(p => p.Price));
    //    Console.WriteLine(products.Min(p => p.Price));
    //    Console.WriteLine(products.Max(p => p.Price));

    //    // QUERY SYNTAX
    //    var query3 = from p in products
    //                 select p.Price;

    //    Console.WriteLine(query3.Average());
    //    Console.WriteLine(query3.Min());
    //    Console.WriteLine(query3.Max());


    //    // Task 4: Total inventory value (Price * Stock).

    //    // METHOD SYNTAX
    //    var result3 = products.Sum(p => p.Price * p.Stock);
    //    Console.WriteLine(result3);

    //    // QUERY SYNTAX
    //    var query4 = from p in products
    //                 select p.Price * p.Stock;

    //    Console.WriteLine(query4.Sum());
    //}

    //static void Exercise07_AnyAll()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: Any product costs more than 50000?

    //    // METHOD SYNTAX
    //    var result1 = products.Any(p => p.Price > 50000);
    //    Console.WriteLine(result1);

    //    // QUERY SYNTAX
    //    var query1 = from p in products
    //                 where p.Price > 50000
    //                 select p;

    //    Console.WriteLine(query1.Any());


    //    // Task 2: All products have non-negative stock?

    //    // METHOD SYNTAX
    //    var result2 = products.All(p => p.Stock >= 0);
    //    Console.WriteLine(result2);

    //    // QUERY SYNTAX
    //    var query2 = from p in products
    //                 where p.Stock < 0
    //                 select p;

    //    Console.WriteLine(!query2.Any());
    //}

    //static void Exercise08_Grouping()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: Group products by Category.

    //    // METHOD SYNTAX
    //    var result1 = products.GroupBy(p => p.Category);

    //    foreach (var group in result1)
    //    {
    //        Console.WriteLine(group.Key);

    //        foreach (var p in group)
    //            Console.WriteLine(p.ProductId + " " + p.Price);
    //    }

    //    // QUERY SYNTAX
    //    var result2 = from p in products
    //                  group p by p.Category;

    //    foreach (var group in result2)
    //    {
    //        Console.WriteLine(group.Key);

    //        foreach (var p in group)
    //            Console.WriteLine(p.ProductId + " " + p.Price);
    //    }


    //    // Task 2: Count, total stock and average price.

    //    // METHOD SYNTAX
    //    var result3 = products.GroupBy(p => p.Category)
    //        .Select(g => new
    //        {
    //            Category = g.Key,
    //            Count = g.Count(),
    //            TotalStock = g.Sum(p => p.Stock),
    //            AveragePrice = g.Average(p => p.Price)
    //        });

    //    foreach (var item in result3)
    //    {
    //        Console.WriteLine(
    //            item.Category + " " +
    //            item.Count + " " +
    //            item.TotalStock + " " +
    //            item.AveragePrice);
    //    }

    //    // QUERY SYNTAX
    //    var result4 = from p in products
    //                  group p by p.Category into g
    //                  select new
    //                  {
    //                      Category = g.Key,
    //                      Count = g.Count(),
    //                      TotalStock = g.Sum(p => p.Stock),
    //                      AveragePrice = g.Average(p => p.Price)
    //                  };

    //    foreach (var item in result4)
    //    {
    //        Console.WriteLine(
    //            item.Category + " " +
    //            item.Count + " " +
    //            item.TotalStock + " " +
    //            item.AveragePrice);
    //    }
    //}

    //static void Exercise09_SetOperations()
    //{
    //    var a = StoreData.ListA;
    //    var b = StoreData.ListB;

    //    // Task 1: Remove duplicates.

    //    // METHOD SYNTAX
    //    var result1 = a.Distinct();
    //    foreach (var item in result1)
    //        Console.WriteLine(item);

    //    // QUERY SYNTAX
    //    var query1 = from item in a
    //                 select item;

    //    foreach (var item in query1.Distinct())
    //        Console.WriteLine(item);


    //    // Task 2: Union, intersection and A-only values.

    //    // METHOD SYNTAX
    //    var union = a.Union(b);
    //    var intersection = a.Intersect(b);
    //    var onlyA = a.Except(b);

    //    foreach (var item in union)
    //        Console.WriteLine(item);

    //    foreach (var item in intersection)
    //        Console.WriteLine(item);

    //    foreach (var item in onlyA)
    //        Console.WriteLine(item);


    //    // QUERY SYNTAX
    //    var queryA = from item in a select item;
    //    var queryB = from item in b select item;

    //    foreach (var item in queryA.Union(queryB))
    //        Console.WriteLine(item);

    //    foreach (var item in queryA.Intersect(queryB))
    //        Console.WriteLine(item);

    //    foreach (var item in queryA.Except(queryB))
    //        Console.WriteLine(item);
    //}

    //static void Exercise10_InnerJoin()
    //{
    //    var employees = StoreData.Employees;
    //    var departments = StoreData.Departments;

    //    // METHOD SYNTAX
    //    var result1 = employees.Join(
    //        departments,
    //        e => e.DepartmentId,
    //        d => d.DepartmentId,
    //        (e, d) => new
    //        {
    //            EmployeeName = e.EmployeeName,
    //            DepartmentName = d.DepartmentName
    //        });

    //    foreach (var item in result1)
    //        Console.WriteLine(item.EmployeeName + " " + item.DepartmentName);


    //    // QUERY SYNTAX
    //    var result2 = from e in employees
    //                  join d in departments
    //                  on e.DepartmentId equals d.DepartmentId
    //                  select new
    //                  {
    //                      EmployeeName = e.EmployeeName,
    //                      DepartmentName = d.DepartmentName
    //                  };

    //    foreach (var item in result2)
    //        Console.WriteLine(item.EmployeeName + " " + item.DepartmentName);
    //}

    //static void Exercise11_LeftJoin()
    //{
    //    var employees = StoreData.Employees;
    //    var departments = StoreData.Departments;

    //    // METHOD SYNTAX: GroupJoin + SelectMany
    //    var result1 = employees.GroupJoin(
    //        departments,
    //        e => e.DepartmentId,
    //        d => d.DepartmentId,
    //        (e, deptGroup) => new { e, deptGroup })
    //        .SelectMany(
    //            x => x.deptGroup.DefaultIfEmpty(),
    //            (x, d) => new
    //            {
    //                EmployeeName = x.e.EmployeeName,
    //                DepartmentName = d == null
    //                    ? "Unassigned"
    //                    : d.DepartmentName
    //            });

    //    foreach (var item in result1)
    //        Console.WriteLine(item.EmployeeName + " " + item.DepartmentName);


    //    // QUERY SYNTAX
    //    var result2 = from e in employees
    //                  join d in departments
    //                  on e.DepartmentId equals d.DepartmentId
    //                  into deptGroup
    //                  from d in deptGroup.DefaultIfEmpty()
    //                  select new
    //                  {
    //                      EmployeeName = e.EmployeeName,
    //                      DepartmentName = d == null
    //                          ? "Unassigned"
    //                          : d.DepartmentName
    //                  };

    //    foreach (var item in result2)
    //        Console.WriteLine(item.EmployeeName + " " + item.DepartmentName);
    //}

    //static void Exercise12_GroupJoin()
    //{
    //    var employees = StoreData.Employees;
    //    var departments = StoreData.Departments;

    //    // METHOD SYNTAX
    //    var result1 = departments.GroupJoin(
    //        employees,
    //        d => d.DepartmentId,
    //        e => e.DepartmentId,
    //        (d, employeeGroup) => new
    //        {
    //            DepartmentName = d.DepartmentName,
    //            EmployeeCount = employeeGroup.Count()
    //        });

    //    foreach (var item in result1)
    //        Console.WriteLine(item.DepartmentName + " " + item.EmployeeCount);


    //    // QUERY SYNTAX
    //    var result2 = from d in departments
    //                  join e in employees
    //                  on d.DepartmentId equals e.DepartmentId
    //                  into employeeGroup
    //                  select new
    //                  {
    //                      DepartmentName = d.DepartmentName,
    //                      EmployeeCount = employeeGroup.Count()
    //                  };

    //    foreach (var item in result2)
    //        Console.WriteLine(item.DepartmentName + " " + item.EmployeeCount);
    //}

    //static void Exercise13_Partitioning()
    //{
    //    var products = StoreData.Products.OrderBy(p => p.ProductId);

    //    // Task 1: First three products.

    //    // METHOD SYNTAX
    //    var result1 = products.Take(3);
    //    foreach (var p in result1)
    //        Console.WriteLine(p.ProductId);

    //    // QUERY SYNTAX
    //    var query1 = from p in products
    //                 select p;

    //    foreach (var p in query1.Take(3))
    //        Console.WriteLine(p.ProductId);


    //    // Task 2: Skip first three products.

    //    // METHOD SYNTAX
    //    var result2 = products.Skip(3);
    //    foreach (var p in result2)
    //        Console.WriteLine(p.ProductId);

    //    // QUERY SYNTAX
    //    var query2 = from p in products
    //                 select p;

    //    foreach (var p in query2.Skip(3))
    //        Console.WriteLine(p.ProductId);
    //}

    //static void Exercise14_QuantifiersAndMembership()
    //{
    //    List<int> requestedIds = new() { 1, 3, 5 };

    //    var products = StoreData.Products;

    //    // METHOD SYNTAX
    //    var result1 = products.Where(
    //        p => requestedIds.Contains(p.ProductId));

    //    foreach (var p in result1)
    //        Console.WriteLine(p.ProductId);

    //    // QUERY SYNTAX
    //    var result2 = from p in products
    //                  where requestedIds.Contains(p.ProductId)
    //                  select p;

    //    foreach (var p in result2)
    //        Console.WriteLine(p.ProductId);
    //}

    //static void Exercise15_StringAndNumericQueries()
    //{
    //    var products = StoreData.Products;

    //    // Task 1: Names starting with O.

    //    // METHOD SYNTAX
    //    var result1 = products.Where(
    //        p => p.ProductName.StartsWith("O"));

    //    foreach (var p in result1)
    //        Console.WriteLine(p.ProductName);

    //    // QUERY SYNTAX
    //    var result2 = from p in products
    //                  where p.ProductName.StartsWith("O")
    //                  select p;

    //    foreach (var p in result2)
    //        Console.WriteLine(p.ProductName);


    //    // Task 2: Products with even ProductIds.

    //    // METHOD SYNTAX
    //    var result3 = products.Where(
    //        p => p.ProductId % 2 == 0);

    //    foreach (var p in result3)
    //        Console.WriteLine(p.ProductId);

    //    // QUERY SYNTAX
    //    var result4 = from p in products
    //                  where p.ProductId % 2 == 0
    //                  select p;

    //    foreach (var p in result4)
    //        Console.WriteLine(p.ProductId);
    //}

    static void Exercise16_NestedQueries()
    {
        var products = StoreData.Products;
        var orders = StoreData.Orders;

        // Task 1: Find products priced above average.
        // METHOD SYNTAX PRACTICE
        // TODO: Average + Where

        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 2: Find customers with at least one order.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + Any

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise17_CombinedQueries()
    {
        var products = StoreData.Products;

        // Task: Find in-stock Electronics products, sort by descending
        // price, and return only product names.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + OrderByDescending + Select

        // QUERY SYNTAX PRACTICE
        // TODO: from + where + orderby + select
    }

    static void Exercise18_DictionaryAndLookup()
    {
        var products = StoreData.Products;

        // Task 1: Create a dictionary keyed by ProductId.
        // METHOD SYNTAX PRACTICE
        // TODO: ToDictionary()

        // QUERY SYNTAX PRACTICE
        // TODO: query + ToDictionary()

        // Task 2: Create a lookup by Category.
        // METHOD SYNTAX PRACTICE
        // TODO: ToLookup()

        // QUERY SYNTAX PRACTICE
        // TODO: query + ToLookup()
    }

    static void Exercise19_JoinOrdersAndCustomers()
    {
        var orders = StoreData.Orders;
        var customers = StoreData.Customers;

        // Task: Show OrderId, CustomerName, OrderDate and Status.
        // METHOD SYNTAX PRACTICE
        // TODO: Join + Select

        // QUERY SYNTAX PRACTICE
        // TODO: join + select
    }

    static void Exercise20_Reports()
    {
        var products = StoreData.Products;
        var orders = StoreData.Orders;

        // Task 1: Count orders by Status.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy + Count

        // QUERY SYNTAX PRACTICE
        // TODO: group by + Count

        // Task 2: Create a category report with product count,
        // average price, total stock and inventory value.
        // METHOD SYNTAX PRACTICE
        // TODO

        // QUERY SYNTAX PRACTICE
        // TODO
    }
}
