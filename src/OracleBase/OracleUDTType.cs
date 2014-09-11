using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Reflection;
using greyleader.DataAccess.Oracle.OracleExtensions;


namespace greyleader.DataAccess.Oracle
{
	public abstract class OracleUDTType<T> : INullable, IOracleCustomType, IOracleCustomTypeFactory
		where T : OracleUDTType<T>, new()
	{
		private bool _isNull;

		public virtual bool IsNull
		{
			get
			{
				return _isNull;
			}
			protected set
			{
				_isNull = value;
			}
		}

		public static T Null
		{
			get
			{
				T t = new T();
				t._isNull = true;
				return t;
			}
		}

		public IOracleCustomType CreateObject()
		{
			return new T();
		}

		public void FromCustomObject(OracleConnection con, IntPtr pUdt)
		{
			//Note: we have to cast to (T) so that the extension method can get a hold of the right type info
			((T)this).SetUdt(con, pUdt);
		}

		public void ToCustomObject(OracleConnection con, IntPtr pUdt)
		{
			//Note: we have to cast to (T) so that the extension method can get a hold of the right type info
			((T)this).GetUdt(con, pUdt);

		}
	}
}
