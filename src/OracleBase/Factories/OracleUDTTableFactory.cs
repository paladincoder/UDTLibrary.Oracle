using System;
using Oracle.DataAccess.Types;

namespace greyleader.DataAccess.Oracle
{
    public class OracleUDTTableFactory<T> : IOracleCustomTypeFactory, IOracleArrayTypeFactory where T : IOracleCustomType, new()
    {
        public virtual IOracleCustomType CreateObject()
        {
            T obj = new T();
            return obj;
        }

        public virtual System.Array CreateArray(int length)
        {
            System.Type type = typeof(T).GetProperties()[0].PropertyType.GetElementType();
            return Array.CreateInstance(type, length);
        }

        public virtual System.Array CreateStatusArray(int length)
        {
            return null;
        }
    }
}
