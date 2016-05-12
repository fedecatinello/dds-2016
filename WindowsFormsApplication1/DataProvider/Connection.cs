using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security;

namespace WindowsFormsApplication1.DataProvider
{
    class Connection
    {
        
       //DB connection singleton instance
       private SqlConnection gd_connection; 

        //Get DB connection
       public SqlConnection getConnection() 
       {
            
            if(gd_connection == null)
                return new SqlConnection("gd1C2016");

            return gd_connection;        
        }

        //Get DB connection with credentials
       public SqlConnection getSecuredConnection()
       {
           if (gd_connection == null)
               return new SqlConnection("gd1C2016", getCredentials());

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

            return new SqlCredential("gd", password);

        }
        
        
    }
}
