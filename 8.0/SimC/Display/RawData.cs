using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace SimC
{
    public class RawData
    {
        public RawData()
        {
        }

        public bool Load(string filename)
        {
            string path = Path.GetDirectoryName(filename);
            string file = Path.GetFileName(filename);
            string url  = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Extended Properties=\"Text;HDR=No;FMT=Delimited\"";

            OleDbConnection connection = new OleDbConnection(url);
            connection.Open();

            string query = $"select * from {file}";

            DataTable table = new DataTable();
            OleDbDataAdapter oleData = new OleDbDataAdapter(query, connection);
            if (oleData != null)
            {
                oleData.Fill(table);
            }

            //DataView dv = new DataView(table);
            //DataRow[] rows = table.Select("F4 = 'Rake'");

            connection.Close();
            return true;
        }


    }
}
