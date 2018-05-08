UDTLibrary
==========

.Net 2.0 --

A library to help mapping Oracle UDT POCO classes with Database User Defined Types

This project assists when creating a POCO to map to an Oracle UDT using the [OracleObjectMappingAttribute].

As this creates a static object that keeps track of fields of types and their corresponding Oracle UDT mapping type, 
this runs fast after the first mapping.

When communicating with Oracle stored procedures, it is more resillient to communicate with UDTs when working with distributed teams, 
as the Oracle developer can modify (add) fields to an existing UDT without breaking the C# client using this library. When the Oracle
developer has completed his/her work, the new field name can be mapped with the attribute, and all other information is handled by this library.


<h1>Example</H1>
Here's an example of what this library will do. Let's say we have 2 types defined in Oracle.
```SQL
create or replace type order_obj as object
(       customer_name  varchar2(30),
        address        varchar2(100), 
        order_date     date, 
        qty            number, 
        price          number
)
create or replace type order_list as table of order_obj;
```

To map them with this library, create a poco, referencing of course the Oracle.DataAccess assembly and using the `[OracleObjectMappingAttribute]`. Note, the values of the mapping parameters need to be upper case.

```C#
    [OracleCustomTypeMappingAttribute("{schema}.ORDER_OBJ")]
    public class Order : OracleUDTType<Order>
    {
      [OracleObjectMappingAttribute("CUSTOMER_NAME")]
      public string CustomerName {get;set;}
  
      [OracleObjectMappingAttribute("ADDRESS")]
      public string Address {get;set;}
  
      [OracleObjectMappingAttribute("ORDER_DATE")]
      public DateTime OrderDate {get;set;}
  
      [OracleObjectMappingAttribute("QTY")]
      public int Quantity{get;set;}
  
      [OracleObjectMappingAttribute("PRICE")]
      public int Price {get;set;}
    }
```


The table of orders would be similar in structure.
Note: At time of posting, I do not have an oracle db with which I can test the table with at the moment. You may need to add a seperate class for the table factory. 

```C#
    public class OrderList : OracleUDTTable<OrderList>
    {}

    [OracleCustomTypeMappingAttribute("{schema}.ORDER_LIST")]
    public class OrderListFactory : OracleUDTTableFactory<OrderList>
    {}
```

The base classes will then take care of all the creating and mapping between the UDT and the POCO object. All that's required now is to properly set the `UDTType` property on the `OracleCommand` object.
