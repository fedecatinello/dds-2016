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

namespace MercadoEnvio.Login
{
    public partial class ElegirRol : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        
        public Object SelectedItem { get; set; }

        public ElegirRol()
        {
            InitializeComponent();
            
        }

        private void ElegirRol_Load(object sender, EventArgs e)
        {
            CargarRoles();
        }

        private void CargarRoles()
        {
            DataSet roles = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@username", UsuarioSesion.usuario.nombre));
            //command = QueryBuilder.Instance.build("SELECT r.nombre from NET_A_CERO.Rol r, NET_A_CERO.Usuarios_x_Rol ru WHERE r.habilitado = 1 AND ru.habilitado = 1 AND (SELECT id FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username) = ru.usr_id and r.id = ru.rol_id ", parametros);
            command = QueryBuilder.Instance.build("SELECT r.rol_nombre from NET_A_CERO.Rol r, NET_A_CERO.Usuarios_x_Rol ru WHERE r.rol_activo = 1 AND (SELECT usr_id FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username) = ru.usr_id AND r.rol_id = ru.rol_id ", parametros);
            adapter.SelectCommand = command;
            adapter.Fill(roles, "Rol");
            comboBoxRol.DataSource = roles.Tables[0].DefaultView;
            comboBoxRol.ValueMember = "nombre";
        }

        private void botonAceptar_Click(object sender, EventArgs e)
        {
            String rolElegido = comboBoxRol.SelectedValue.ToString();
            UsuarioSesion.Usuario.rol = rolElegido;

            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

    }
}