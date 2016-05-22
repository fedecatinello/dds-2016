using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MercadoEnvio.Objetos
{
    interface Comunicable
    {
        String GetQueryCrear();
        String GetQueryModificar();
        String GetQueryObtener();
        void CargarInformacion(SqlDataReader reader);
        IList<SqlParameter> GetParametros();
    }
}