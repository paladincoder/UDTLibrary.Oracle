using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace greyleader.DataAccess.Oracle
{
    public abstract class OracleUDTTable<T> : IOracleCustomType
    {
        [OracleArrayMappingAttribute()]
        public virtual T[] Value
        {
            get;
            set;
        }

        public virtual void FromCustomObject(OracleConnection con, System.IntPtr pUdt)
        {
            if (this.Value != null)
            {
                OracleUdt.SetValue(con, pUdt, 0, this.Value);
            }
        }

        public virtual void ToCustomObject(OracleConnection con, System.IntPtr pUdt)
        {
            this.Value = ((T[])(OracleUdt.GetValue(con, pUdt, 0)));
        }
    }
}
