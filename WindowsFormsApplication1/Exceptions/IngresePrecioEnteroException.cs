using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoEnvio.Exceptions
{
    class IngresePrecioEnteroException : Exception
    {
        public IngresePrecioEnteroException(String mensaje)
            : base(mensaje)
        {

        }
    }
}
