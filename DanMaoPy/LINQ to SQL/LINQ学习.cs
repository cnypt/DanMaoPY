Where 操作
1。简单形式
    var q = 
        from c in db.Customers
        where c.City == "London"
        select c;
   var q =
    from e in db.Employees
    where e.HireDate >= new DateTime(1994, 1, 1)
    select e;
2.关系条件形式：
    var q =
    from p in db.Products
    where p.UnitsInStock <= p.ReorderLevel && !p.Discontinued
    select p;
3.First()形式
    Customer cust = db.Customers.First(c=>c.CustomerID == "BONAP");
Select/Distinct操作符
1.简单形式
    var q =
    from c in db.Customers
    select c.ContactName;
2.匿名类型形式
    var q =
    from e in db.Employees
    select new
    {
        Name = e.FirstName + " " + e.LastName,
        Phone = e.HomePhone
    };
3.条件形式
    var q =
    from p in db.Products
    select new
    {
        p.ProductName,
        Availability =
        p.UnitsInStock - p.UnitsOnOrder < 0 ? 
        "Out Of Stock" : "In Stock"
    };
4.指定类型形式
    var q =
    from e in db.Employees
    select new Name
    {
        FirstName = e.FirstName,
        LastName = e.LastName
    };
5.筛选形式
    var q =
    from c in db.Customers
    where c.City == "London"
    select c.ContactName;
6.shaped形式(整形类型)：
说明：其select操作使用了匿名对象，而这个匿名对象中，其属性也是个匿名对象。
var q =
    from c in db.Customers
    select new {
        c.CustomerID,
        CompanyInfo = new {c.CompanyName, c.City, c.Country},
        ContactInfo = new {c.ContactName, c.ContactTitle}
    };
7.嵌套类型形式：
说明：返回的对象集中的每个对象DiscountedProducts属性中，又包含一个集合。也就是每个对象也是一个集合类。
var q =
    from o in db.Orders
    select new {
        o.OrderID,
        DiscountedProducts =
            from od in o.OrderDetails
            where od.Discount > 0.0
            select od,
        FreeShippingDiscount = o.Freight
    };  
8.本地方法调用形式(LocalMethodCall)：
这个例子在查询中调用本地方法PhoneNumberConverter将电话号码转换为国际格式。
var q = from c in db.Customers
         where c.Country == "UK" || c.Country == "USA"
         select new
         {
             c.CustomerID,
             c.CompanyName,
             Phone = c.Phone,
             InternationalPhone = 
             PhoneNumberConverter(c.Country, c.Phone)
         };
PhoneNumberConverter方法如下：
public string PhoneNumberConverter(string Country, string Phone)
{
    Phone = Phone.Replace(" ", "").Replace(")", ")-");
    switch (Country)
    {
        case "USA":
            return "1-" + Phone;
        case "UK":
            return "44-" + Phone;
        default:
            return Phone;
    }
}
9.Distinct形式：
说明：筛选字段中不相同的值。用于查询不重复的结果集。生成SQL语句为：SELECT DISTINCT [City] FROM [Customers]
var q = (
    from c in db.Customers
    select c.City )
    .Distinct();
Count/Sum/Min/Max/Avg操作符   
1.简单形式：
得到数据库中客户的数量：
var q = db.Customers.Count();
2.带条件形式：
得到数据库中未断货产品的数量：
var q = db.Products.Count(p => !p.Discontinued);  
1.简单形式：
得到所有订单的总运费：
var q = db.Orders.Select(o => o.Freight).Sum();
2.映射形式：
得到所有产品的订货总数：
var q = db.Products.Sum(p => p.UnitsOnOrder);
1.简单形式：
查找任意产品的最低单价：
var q = db.Products.Select(p => p.UnitPrice).Min();
2.映射形式：
查找任意订单的最低运费：
var q = db.Orders.Min(o => o.Freight);
3.元素：
查找每个类别中单价最低的产品：
var categories =
    from p in db.Products
    group p by p.CategoryID into g
    select new {
        CategoryID = g.Key,
        CheapestProducts =
            from p2 in g
            where p2.UnitPrice == g.Min(p3 => p3.UnitPrice)
            select p2
    };
1.简单形式：
查找任意雇员的最近雇用日期：
var q = db.Employees.Select(e => e.HireDate).Max();
2.映射形式：
查找任意产品的最大库存量：
var q = db.Products.Max(p => p.UnitsInStock);
3.元素：
查找每个类别中单价最高的产品：
var categories =
    from p in db.Products
    group p by p.CategoryID into g
    select new {
        g.Key,
        MostExpensiveProducts =
            from p2 in g
            where p2.UnitPrice == g.Max(p3 => p3.UnitPrice)
            select p2
    };    
Average
说明：返回集合中的数值类型元素的平均值。集合应为数字类型集合，其返回值类型为double；不延迟。生成SQL语句为：SELECT AVG(…) FROM
1.简单形式：
得到所有订单的平均运费：
var q = db.Orders.Select(o => o.Freight).Average();
2.映射形式：
得到所有产品的平均单价：
var q = db.Products.Average(p => p.UnitPrice);
3.元素：
查找每个类别中单价高于该类别平均单价的产品：
var categories =
    from p in db.Products
    group p by p.CategoryID into g
    select new {
        g.Key, 
        ExpensiveProducts =
            from p2 in g
            where p2.UnitPrice > g.Average(p3 => p3.UnitPrice)
            select p2
    };   
GroupJoin
像上面所说的，没有join和into，被翻译成SelectMany，同时有join和into时，那么就被翻译为GroupJoin。在这里into的概念是对其结果进行重新命名。
1.双向联接(Two way join)：
此示例显式联接两个表并从这两个表投影出结果：
var q =
    from c in db.Customers
    join o in db.Orders on c.CustomerID
    equals o.CustomerID into orders
    select new
    {
        c.ContactName,
        OrderCount = orders.Count()
    };
说明：在一对多关系中，左边是1，它每条记录为c（from c in db.Customers），右边是Many，其每条记录叫做o ( join o in db.Orders )，每对应左边的一个c，就会有一组o，那这一组o，就叫做orders，也就是说，我们把一组o命名为orders，这就是into用途。这也就是为什么在select语句中，orders可以调用聚合函数Count。在T-SQL中，使用其内嵌的T-SQL返回值作为字段值。   
生成SQL语句为：
SELECT [t0].[ContactName], (
    SELECT COUNT(*)
    FROM [dbo].[Orders] AS [t1]
    WHERE [t0].[CustomerID] = [t1].[CustomerID]
) AS [OrderCount]
FROM [dbo].[Customers] AS [t0]   
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    