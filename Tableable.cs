using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor
{
    public abstract class Tableable
    {
        protected abstract DataRow GenerateRow(DataRow row);
        public abstract DataColumn[] GetColumns();

        public DataTable GetRow(DataTable? tb = null)
        {
            if (tb == null)
            {
                tb = GetTable();
            }
            tb.Rows.Add(GenerateRow(tb.NewRow()));
            return tb;
        }
        
        public DataTable GetTable()
        {
            var tb = new DataTable();
            var cols = GetColumns();
            tb.Columns.AddRange(cols);
            return tb;
        }
    }
}
