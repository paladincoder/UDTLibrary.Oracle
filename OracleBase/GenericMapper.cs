using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace greyleader.DataAccess.Oracle.OracleExtensions
{
	/// <summary>
	/// translated from
	/// http://stackoverflow.com/questions/12914990/getcustomattributes-performance-issue-expression-trees-is-the-solution
	/// </summary>
	internal static class UdtExtensions
	{
		public static void GetUdt<T>(this T me, OracleConnection con, IntPtr pUdt)
		{
			GenericMapper<T>.MapTo(me, con, pUdt);
		}
		public static void SetUdt<T>(this T me, OracleConnection con, IntPtr pUdt)
		{
			GenericMapper<T>.MapFrom(me, con, pUdt);
		}

		private static class GenericMapper<T>
		{

			static readonly List<ItemMapper> _mappers;
			/// <summary>
			/// first access of this class will create a static list of the property mappings, and will remain until
			/// app pool is recycled/restarted
			/// </summary>
			static GenericMapper()
			{
				_mappers = typeof(T)
					.GetProperties()
					.Where(p => p.IsDefined(typeof(OracleObjectMappingAttribute), true)).Select(p => new
					{
						Property = p,
						Attribute = p.GetCustomAttributes(typeof(OracleObjectMappingAttribute), false)
									  .Cast<OracleObjectMappingAttribute>()
									  .FirstOrDefault()
					})
					.Select(m => new ItemMapper(m.Property, m.Attribute))
					.ToList();
			}
			public static void MapTo(T me, OracleConnection con, IntPtr pUdt)
			{
				foreach (var mapper in _mappers)
				{
					mapper.MapTo(me, con, pUdt);
				}
			}
			public static void MapFrom(T me, OracleConnection con, IntPtr pUdt)
			{
				foreach (var mapper in _mappers)
				{
					mapper.MapFrom(me, con, pUdt);
				}
			}
			#region Nested type: ItemMapper

			private sealed class ItemMapper
			{
				private readonly OracleObjectMappingAttribute _attribute;
				private readonly PropertyInfo _property;

				public ItemMapper(PropertyInfo property, OracleObjectMappingAttribute attribute)
				{
					_property = property;
					_attribute = attribute;
				}

				public void MapTo(T instance, OracleConnection conn, IntPtr pUdt)
				{

					if (_attribute != null)
					{
						string attributeName = _attribute.AttributeName;
						if (!OracleUdt.IsDBNull(conn, pUdt, attributeName))
						{
							object value = OracleUdt.GetValue(conn, pUdt, attributeName);

							if (!(value == null || value is DBNull))
							{
								_property.SetValue(instance, value, null);
							}
						}
					}
				}

				internal void MapFrom(T instance, OracleConnection con, IntPtr pUdt)
				{
					if (_attribute != null)
					{
						var value = _property.GetValue(instance, null);
						
						var attributeName = _attribute.AttributeName;
						
						if (value != null)
						{
							//Attempted to read or write protected memory. 
							//if string.empy is set on a UDT property, then the client throws a fit, set string.empty to null value.
							if (_property.PropertyType == typeof(string) && string.IsNullOrEmpty(value.ToString()))
								value = null;
							OracleUdt.SetValue(con, pUdt, attributeName, value);
						}
					}
				}
			}

			#endregion
		}
	}
}