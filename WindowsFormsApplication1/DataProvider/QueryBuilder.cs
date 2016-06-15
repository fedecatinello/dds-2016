using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;


namespace MercadoEnvio.DataProvider
{
    class QueryBuilder
    {

        /// <summary>
        /// Singleton attribute
        /// </summary>
        private static QueryBuilder instance;

        public static QueryBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QueryBuilder();
                }
                return instance;
            }
        }

        private SqlCommand command { get; set; }
        private ConnectionManager conexion = new ConnectionManager();

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
            if (this.command.Connection == null) this.command.Connection = conexion.connect();

            return this.command;
        }
    }
}