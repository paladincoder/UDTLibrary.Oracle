UDTLibrary
==========

A library to help mapping Oracle UDT POCO classes with Database User Defined Types

This project assists when creating a POCO to map to an Oracle UDT using the [OracleObjectMappingAttribute].

As this creates a static object that keeps track of fields of types and their corresponding Oracle UDT mapping type, 
this runs fast after the first mapping.

When communicating with Oracle stored procedures, it is more resillient to communicate with UDTs when working with distributed teams, 
as the Oracle developer can modify (add) fields to an existing UDT without breaking the C# client using this library. When the Oracle
developer has completed his/her work, the new field name can be mapped with the attribute, and all other information is handled by this library.
