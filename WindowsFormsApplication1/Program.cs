using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercadoEnvio
{
    static class Program
    {
        /// <summary>
        /// Punto de ingreso a la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Comprar_Ofertar.VerPublicacion(71080));
            //Application.Run(new Comprar_Ofertar.VerPublicacion(71079));
            //Application.Run(new Comprar_Ofertar.Ofertar(100,71080));
            Application.Run(new Comprar_Ofertar.Comprar(2,66139,123));
            
            
        }
    }
}
