using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        public String server { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String database { get; set; }
        public String schema { get; set; }

        private DateTime currentDate;

        private Config()
        {
            parseConfig();
        }
        private void parseConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../../configuracion.xml");

            XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("properties");

            foreach (XmlNode node in nodes)
            {
                server = node.SelectSingleNode("server").InnerText;
                username = node.SelectSingleNode("username").InnerText;
                password = node.SelectSingleNode("password").InnerText;
                database = node.SelectSingleNode("database").InnerText;
                schema = node.SelectSingleNode("schema").InnerText;
            }
            currentDate = DateTime.Parse(doc.DocumentElement.GetElementsByTagName("time")[0].SelectSingleNode("current/time").InnerText);
        }

        internal DateTime getCurrentDate()
        {
            return currentDate;
        }

        public string getDateFormat()
        {
            return "dmy";
        }
    }
}
