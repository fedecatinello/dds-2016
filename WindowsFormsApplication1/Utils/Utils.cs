using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.Common
{
    class Utils
    {
        
        /// <summary>
        /// Singleton attribute
        /// </summary>
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
        }

        /// <summary>Load the combobox executing a query with the given parameters</summary>
        /// <param name="entidad">Name of the table</param>
        /// <param name="atributo">Name of the field to select</param>
        /// <param name="comboBox">Name of the combo box to insert the results</param>
        /// 
        
        /*-------------Comento LoadComboBox, no se usa------------------------*/
        /*
         public void loadComboBox(String entity, String attribute, ComboBox comboBox)
        {       
            String queryCombo = "SELECT DISTINCT " + attribute + 
                                " FROM " + ConnectionManager.Instance.getSchema() + " ." + entity + " AS " + entity;
            
            SqlDataReader response_buffer = QueryHelper.Instance.exec(queryCombo);

            while (QueryHelper.Instance.readFrom(response_buffer))
            {
                comboBox.Items.Add(response_buffer[attribute].ToString());
            }
            
            QueryHelper.Instance.closeReader();
        }
        */

        /// <summary>Load the combobox executing a query with the given parameters, using a where statement</summary>
        /// <param name="entidad">Name of the table</param>
        /// <param name="atributo">Name of the field to select</param>
        /// <param name="comboBox">Name of the combo box to insert the results</param>
        /// <param name="where">Condition to include in the query</param>
        /// 
        /*-------------Comento cargarComboBoxWhere, no se usa------------------------*/
        /*
        public void cargarComboBoxWhere(String entity, String attribute, ComboBox comboBox, String whereCondition)
        {
            String queryCombo = "SELECT DISTINCT " + attribute + 
                                    " FROM " + ConnectionManager.Instance.getSchema() + "." + entity + " AS " + entity + 
                                    " WHERE " + whereCondition;
            
            SqlDataReader buffer = QueryHelper.Instance.exec(queryCombo);

            while (QueryHelper.Instance.readFrom(buffer))
            {
                comboBox.Items.Add(buffer[attribute].ToString());
            }

            QueryHelper.Instance.closeReader();
        }
        */

        /// <summary>Load the combobox executing a query with the given parameters, using the order by statement</summary>
        /// <param name="entidad">Name of the table</param>
        /// <param name="atributo">Name of the field to select</param>
        /// <param name="comboBox">Name of the combo box to insert the results</param>
        /// 
        /*-------------Comento cargarComboBoxOrderBy, no se usa------------------------*/
        /*
        public void cargarComboBoxOrderBy(String entity, String attribute, ComboBox comboBox)
        {         
            String queryCombo = "SELECT DISTINCT " + attribute + 
                                    " FROM " + ConnectionManager.Instance.getSchema() + "." + entity + 
                                    " ORDER BY " + attribute;
            
            SqlDataReader response_buffer = QueryHelper.Instance.exec(queryCombo);

            while (QueryHelper.Instance.readFrom(response_buffer))
            {
                comboBox.Items.Add(response_buffer[attribute].ToString());
            }

            QueryHelper.Instance.closeReader();
        }
        */


        /// <summary>Select an ID from a table of a specific attribute</summary>
        /// <param name="function">The SQL function to execute</param>
        /// <param name="atributo">Name of the field to get the ID</param>
        /// <returns>The id of the given field in the table<returns>
        /// 
        /*-------------Comento getIDFrom, no se usa------------------------*/
        /*
        public int getIDFrom(String function, String attribute)
        {
            String query = "SELECT " + ConnectionManager.Instance.getSchema() + "." + function + "('%" + attribute + "%') AS id";

            SqlDataReader buffer = QueryHelper.Instance.exec(query);

            QueryHelper.Instance.readFrom(buffer);
            
            int id = int.Parse(buffer["id"].ToString());
            
            QueryHelper.Instance.closeReader();
            
            return id;
        }
        */


        /// <summary>Get the selected row in a DataGridView</summary>
        /// <param name="dataGrid">The data grid to get the selected row</param>
        /// <returns>The selected row<returns>
        public DataGridViewRow getSelectedRow(DataGridView dataGrid)
        {
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
                return row;

            return null;
        }



    }
}
