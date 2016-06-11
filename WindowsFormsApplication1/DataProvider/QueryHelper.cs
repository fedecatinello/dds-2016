using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoEnvio.DataProvider
{
    class QueryHelper
    {

        /// <summary>
        /// Singleton attribute
        /// </summary>
        private static QueryHelper instance;

        public static QueryHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QueryHelper();
                }
                return instance;
            }
        }

        /// <summary>Execute a query with a return value (functions or selects)</summary>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the reader with the results of the query</returns>
        public SqlDataReader exec(String query, IList<SqlParameter> parameters)
        {
            SqlCommand command = QueryBuilder.Instance.build(query, parameters);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        public bool readFrom(SqlDataReader reader_parameter)
        {
            return reader_parameter.Read();
        }

        /// <summary>Close the reader opened with the previous method (if the reader is not closed it will fail in the next execution)</summary>
        public void closeReader(SqlDataReader reader)
        {
            reader.Close();
        }

    }
}
