using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WindowsFormsApplication1.DataProvider
{
    class QueryBuilder
    {
        private SqlCommand command { get; set; }

        public SqlCommand build(string sqlText, IList<SqlParameter> parameters)
        {
            this.command = new SqlCommand();
            this.command.CommandText = sqlText;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    this.command.Parameters.Add(parameter);
                }
            }
            
            return this.command;
        }
    }
}