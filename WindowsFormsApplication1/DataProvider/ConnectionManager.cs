using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security;

namespace MercadoEnvio.DataProvider
{
    class ConnectionManager
    {

        private static String CONNECTION_STRING = "gd1C2016";
        private static String SCHEMA = "NET_A_CERO";
        private static String DATABASE = "GD1C2016";
        private static String USER_ID = "gd";

        /// <summary>
        /// Singleton attribute
        /// </summary>
        private static ConnectionManager instance;

        public static ConnectionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectionManager();
                }
                return instance;
            }
        }

        //DB connection instance
        private SqlConnection gd_connection;

        //Get DB connection
        public SqlConnection getConnection()
        {
            if (gd_connection == null)
            {
                connect();
            }
            return gd_connection;
        }


        //Get DB credentials
        public SqlCredential getCredentials()
        {
            SecureString password = new SecureString();
            password.AppendChar('g');
            password.AppendChar('d');
            password.AppendChar('2');
            password.AppendChar('0');
            password.AppendChar('1');
            password.AppendChar('6');

            return new SqlCredential(USER_ID, password);
        }

        //Connect to DB
        public void connect()
        {
            gd_connection = new SqlConnection(CONNECTION_STRING, getCredentials());
            gd_connection.Open();

            new SqlCommand("USE [" + DATABASE + "] ", gd_connection).ExecuteNonQuery();
        }


        //Close DB connection
        public void close()
        {
            gd_connection.Close();
        }

        public String getSchema()
        {
            return SCHEMA;
        }

    }

}
