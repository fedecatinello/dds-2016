using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Rol
{
    public partial class AgregarRol : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        
        public AgregarRol()
        {
            InitializeComponent();
        }

        private void AgregarRol_Load_1(object sender, EventArgs e)
        {
            CargarFuncionalidades();
        }   

        private void CargarFuncionalidades()
        {
            DataSet funcionalidades = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            command = QueryBuilder.Instance.build("SELECT DISTINCT func_nombre FROM NET_A_CERO.Funcionalidades", parametros);
            adapter.SelectCommand = command;
            adapter.Fill(funcionalidades);
            checkedListBoxFuncionalidades.DataSource = funcionalidades.Tables[0].DefaultView;
            checkedListBoxFuncionalidades.ValueMember = "func_nombre";
        }

        private void botonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RolForm().ShowDialog();
            this.Close();
        }

        private void botonGuardar_Click(object sender, EventArgs e)
        {
            String queryRol = "INSERT INTO NET_A_CERO.Roles(rol_nombre, rol_activo) VALUES (@rol, 1)";
            String nombreRol = this.textBoxRol.Text;
            parametros.Clear();
            parametros.Add(new SqlParameter("@rol", nombreRol));
            QueryBuilder.Instance.build(queryRol, parametros).ExecuteNonQuery();

            foreach (DataRowView funcionalidad in this.checkedListBoxFuncionalidades.CheckedItems)
            {
                parametros.Clear();
                parametros.Add(new SqlParameter("@rol", nombreRol));
                parametros.Add(new SqlParameter("@funcionalidad", funcionalidad.Row["func_nombre"] as String));

                String queryRolFuncionalidad = "INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) VALUES ((SELECT func_id FROM NET_A_CERO.Funcionalidades WHERE func_nombre = @funcionalidad), (SELECT  rol_id FROM NET_A_CERO.Roles WHERE rol_nombre = @rol))";
                                
                QueryBuilder.Instance.build(queryRolFuncionalidad, parametros).ExecuteNonQuery();                                
            }
            MessageBox.Show("El rol " + nombreRol + " fue creado");
            BorrarDatosIngresados();
        }

        private void checkedListBoxFuncionalidades_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosIngresados();            
        }

        private void BorrarDatosIngresados()
        {
            textBoxRol.Clear();
            for (int i = 0; i < checkedListBoxFuncionalidades.Items.Count; i++)
            {
                checkedListBoxFuncionalidades.SetItemChecked(i, false);
            }
        }

    }
}