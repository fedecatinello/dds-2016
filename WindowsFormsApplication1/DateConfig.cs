using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;

namespace MercadoEnvio
{
    public class DateConfig
    {
        private static DateConfig instance;
        
        public static DateConfig getInstance()
        {
            if (instance == null)
            {
                instance = new DateConfig();
            }

            return instance;
        }
       
        public DateTime getCurrentDate()
        {
          if(!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CurrentDate"]))
          {
              return DateTime.Parse(ConfigurationManager.AppSettings["CurrentDate"]);
          }

          return DateTime.Now;
        }
    }
}
