using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Rubro
{
    public partial class EditarRubro : Form
    {
        //private String idRubro;
        //private String query;
        //private SqlCommand command;
        //private IList<SqlParameter> parametros = new List<SqlParameter>();

        public EditarRubro(String idRubro)
        {
            InitializeComponent();
            button_Guardar.Visible = false;
            button_Cancelar.Visible = false;
            button_Limpiar.Visible = false;
            label1.Visible = false;
            textBox_Descripcion.Visible = false;
            groupBox1.Visible = false;
            checkBox_Habilitado.Visible = false;
            //this.button1 = new System.Windows.Forms.Button();
            //this.label2 = new System.Windows.Forms.Label();
            //this.label3 = new System.Windows.Forms.Label();
            

           // this.idRubro = idRubro;
        }

        private void EditarRubro_Load(object sender, EventArgs e)
        {
            //CargarDatos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }
        
        /*
        private void CargarDatos()
        {
            query = "SELECT * FROM Rubro WHERE id = @idRubro";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idRubro", idRubro));
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            // Si no se pudo leer, tira excepcion
            if (!reader.Read()) throw new Exception("No se puede leer rubro");
            // Si se puede leer, lo muestra en pantalla
            textBox_Descripcion.Text = Convert.ToString(reader["descripcion"]);
            if (Convert.ToBoolean(reader["habilitado"])) checkBox_Habilitado.Checked = true;
        }
         */
    }
}