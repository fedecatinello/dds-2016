using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;

namespace MercadoEnvio
{
    public class Config
    {
        private static Config instance;
        
        public static Config getInstance()
        {
            if (instance == null)
            {
                instance = new Config();
            }

            return instance;
        }
       
        private DateTime currentDate;

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
