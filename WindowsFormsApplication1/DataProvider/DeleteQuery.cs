using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoEnvio.DataProvider
{
    class DeleteQuery : Query
    {
     public DeleteQuery(Type clazz) : base(clazz) { }
 
         public override string build()
         {
             String query = "DELETE FROM " + getTableName() + this.buildWhere();
             Query.addLog(query);
             return query;
         }
 
         public int exec()
         {
             SqlCommand command = new SqlCommand(build(), ConnectionManager.Instance.getConnection());
             return command.ExecuteNonQuery();
         }
 
    }
}
