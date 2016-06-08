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
    public partial class EditarRol : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        
        public Object SelectedItem { get; set; }
        private String rolAEditar;
        private int estabaDeshabilitado;

        public EditarRol(String rol)
        {
            InitializeComponent();
            rolAEditar = rol;            
            
        }

        private void EditarRol_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            this.labelRolElegido.Text = rolAEditar;
            CargarFuncionalidades();            
            if (estaHabilitado())
            {
                checkBoxEstadoRol.Visible = false;
                estabaDeshabilitado = 0;
            }
            else
            {
                checkBoxEstadoRol.Checked = false;
                estabaDeshabilitado = 1;
            }
        }              

        private void botonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RolForm().ShowDialog();
            this.Close();
        }

        private void botonVolverBusqueda_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ListadoEditarRol().ShowDialog();
            this.Close();
        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxRol.Clear();
            CargarFuncionalidades();
        }

        private void botonGuardar_Click(object sender, EventArgs e)
        {
            if(estabaDeshabilitado == 1 && checkBoxEstadoRol.Checked)
            {
                HabilitarRol();
            }

            AgregarFuncionalidades();
            QuitarFuncionalidades();

            if (this.textBoxRol.Text != "")
            {
                RenombrarRol();
                MessageBox.Show("El rol " + rolAEditar + " fue modificado correctamente");
                rolAEditar = this.textBoxRol.Text;
            }
            else
            {
                MessageBox.Show("El rol " + rolAEditar + " fue modificado correctamente");
            }

            CargarDatos();
            textBoxRol.Clear();
            
        }

        private bool estaHabilitado()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@rol", rolAEditar));

            String consulta = "SELECT COUNT(DISTINCT rol_nombre) FROM NET_A_CERO.Roles WHERE rol_nombre = @rol and rol_activo = 1";
            int estadoRol = (int)QueryBuilder.Instance.build(consulta, parametros).ExecuteScalar();

            if (estadoRol == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            MarcarFuncionalidades();            
        }

        private void MarcarFuncionalidades()
        {
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            List<int> funcionalidadesAMarcar = new List<int>();
            DesmarcarFuncionalidades();

            foreach (DataRowView funcionalidad in this.checkedListBoxFuncionalidades.Items)
            {                
                parametros.Clear();
                parametros.Add(new SqlParameter("@rol", rolAEditar));
                parametros.Add(new SqlParameter("@funcionalidad", funcionalidad.Row["func_nombre"] as String));

                if (verificarSiLaTiene(funcionalidad.Row["func_nombre"] as String))
                {
                    int i = checkedListBoxFuncionalidades.Items.IndexOf(funcionalidad);
                    funcionalidadesAMarcar.Add(i);
                    
                }                
            }

            foreach (int index in funcionalidadesAMarcar)
            {
                checkedListBoxFuncionalidades.SetItemChecked(index, true);
            }
        }

        private void DesmarcarFuncionalidades()
        {
            for (int i = 0; i < checkedListBoxFuncionalidades.Items.Count; i++)
            {
                checkedListBoxFuncionalidades.SetItemChecked(i, false);
            }
        }         
        
        private void RenombrarRol()
        {            
            String nuevoNombreRol = this.textBoxRol.Text;

            parametros.Clear();
            parametros.Add(new SqlParameter("@nombre_viejo", rolAEditar));
            parametros.Add(new SqlParameter("@nombre_nuevo", nuevoNombreRol));

            String queryUpdateRol = "UPDATE NET_A_CERO.Roles SET rol_nombre = @nombre_nuevo WHERE rol_nombre = @nombre_viejo";
            QueryBuilder.Instance.build(queryUpdateRol, parametros).ExecuteNonQuery();
            
            MessageBox.Show("El rol " + rolAEditar + " fue renombrado como " + nuevoNombreRol);            
        }

        private void AgregarFuncionalidades()
        {            

            foreach (DataRowView funcionalidad in this.checkedListBoxFuncionalidades.CheckedItems)
            {
                if (verificarSiLaTiene(funcionalidad.Row["func_nombre"] as String))
                {

                }
                else
                {
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@rol", rolAEditar));
                    parametros.Add(new SqlParameter("@funcionalidad", funcionalidad.Row["func_nombre"] as String));

                    String queryRolXFuncionalidad = "INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) VALUES ((SELECT f.func_id FROM NET_A_CERO.Funcionalidades f WHERE f.func_nombre = @funcionalidad), (SELECT r.rol_id FROM NET_A_CERO.Roles r WHERE r.rol_nombre = @rol))";

                    QueryBuilder.Instance.build(queryRolXFuncionalidad, parametros).ExecuteNonQuery();
                }
            }
        }

        private bool verificarSiLaTiene(String funcionalidad)
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@rol", rolAEditar));
            parametros.Add(new SqlParameter("@funcionalidad", funcionalidad));

            String queryCantidadRolXFuncionalidad = "SELECT COUNT(*) FROM NET_A_CERO.Rol_x_Funcionalidad rxf WHERE rxf.func_id = (SELECT f.func_id FROM NET_A_CERO.Funcionalidades f WHERE f.func_nombre = @funcionalidad) AND rxf.rol_id = (SELECT r.rol_id FROM NET_A_CERO.Roles r WHERE r.rol_nombre = @rol)";
            int tieneLaFuncionalidad = (int)QueryBuilder.Instance.build(queryCantidadRolXFuncionalidad, parametros).ExecuteScalar();

                if (tieneLaFuncionalidad == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        private void QuitarFuncionalidades()
        {            
            
            foreach (DataRowView funcionalidad in this.checkedListBoxFuncionalidades.Items)
            {
                int index = checkedListBoxFuncionalidades.Items.IndexOf(funcionalidad);
                String estado = this.checkedListBoxFuncionalidades.GetItemCheckState(index).ToString();

                if (estado == "Unchecked")
                {
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@rol", rolAEditar));
                    parametros.Add(new SqlParameter("@funcionalidad", funcionalidad.Row["func_nombre"] as String));

                    String queryBorrarRolXFuncionalidad = "DELETE NET_A_CERO.Rol_x_Funcionalidad WHERE func_id = (SELECT f.func_id FROM NET_A_CERO.Funcionalidades f WHERE f.func_nombre = @funcionalidad) AND rol_id = (SELECT r.rol_id FROM NET_A_CERO.Roles r WHERE r.rol_nombre = @rol)";

                    QueryBuilder.Instance.build(queryBorrarRolXFuncionalidad, parametros).ExecuteNonQuery();
                }
            }
        }

        private void HabilitarRol()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@nombre", rolAEditar));

            String queryHabilitarRol = "UPDATE NET_A_CERO.Roles SET rol_activo = 1 WHERE rol_nombre = @nombre";

            QueryBuilder.Instance.build(queryHabilitarRol, parametros).ExecuteNonQuery();
        }
               
        
    }
}
