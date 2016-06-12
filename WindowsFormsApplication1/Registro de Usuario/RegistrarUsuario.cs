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

namespace MercadoEnvio.Registro_de_Usuario
{
    public partial class RegistrarUsuario : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private DBMapper mapper = new DBMapper();

        public Object SelectedItem { get; set; }

        public RegistrarUsuario()
        {
            InitializeComponent();
        }

        private void RegistrarUsuario_Load(object sender, EventArgs e)
        {
            CargarRoles();
        }

        private void CargarRoles()
        {
            DataSet roles = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            command = QueryBuilder.Instance.build("SELECT DISTINCT rol_nombre FROM NET_A_CERO.Roles WHERE rol_activo = 1 AND rol_nombre != 'Administrador'", parametros);
            adapter.SelectCommand = command;
            adapter.Fill(roles, "Rol");
            comboBoxRol.DataSource = roles.Tables[0].DefaultView;
            comboBoxRol.ValueMember = "rol_nombre";
            comboBoxRol.SelectedIndex = -1;
        }

        private void botonSiguiente_Click(object sender, EventArgs e)
        {

            String rolElegido = this.comboBoxRol.Text;
            String usuario = this.textBoxUsuario.Text;
            String contraseña = this.textBoxPass.Text;
            String repetirContraseña = this.textBoxPass2.Text;

            if (usuario == "")
            {
                MessageBox.Show("El campo Usuario es obligatorio");
                return;
            }

            if (contraseña == "")
            {
                MessageBox.Show("El campo Contraseña es obligatorio");
                return;
            }
            if (repetirContraseña == "")
            {
                MessageBox.Show("El campo Repetir contraseña es obligatorio");
                return;
            }

            if (rolElegido == "")
            {
                MessageBox.Show("El rol es obligatorio");
                return;
            }

            if (contraseña.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener por lo menos 8 caracteres");
                return;
            }

            if (textBoxPass.Text != textBoxPass2.Text)
            {
                MessageBox.Show("La contraseña no coincide");
                return;
            }

            parametros.Clear();
            parametros.Add(new SqlParameter("@username", usuario));

            // Buscamos si el username ya se encuentra registrado
            String queryUsuario = "SELECT usr_id FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username";

            SqlDataReader reader = QueryBuilder.Instance.build(queryUsuario, parametros).ExecuteReader();

            if (reader.Read())
            {
                MessageBox.Show("El usuario ya existe");
                return;
            }

            if (rolElegido == "Cliente")
            {
                this.Hide();
                new ABM_Cliente.AgregarCliente(usuario,contraseña).ShowDialog();

                if (UsuarioSesion.Usuario.rol != "Administrativo")
                {
                    UsuarioSesion.Usuario.rol = "Cliente";
                    UsuarioSesion.Usuario.nombre = usuario;

                    String idUsuario = "SELECT TOP 1 usr_id" 
                                + " FROM NET_A_CERO.Usuarios"
                                + " ORDER BY usr_id DESC";

                    // Limpio parametros
                    parametros.Clear();

                    int idCliente = (int)QueryBuilder.Instance.build(idUsuario, parametros).ExecuteScalar();

                    UsuarioSesion.Usuario.id = idCliente;
                }
   
            }
            else if (rolElegido == "Empresa")
            {
                new ABM_Empresa.AgregarEmpresa(usuario, contraseña).ShowDialog();

            }
            else
            {
                Int32 idUsuario = mapper.CrearUsuarioConValores(usuario, contraseña);
                mapper.AsignarRolAUsuario(idUsuario, rolElegido);
                UsuarioSesion.Usuario.rol = rolElegido;
                UsuarioSesion.Usuario.nombre = usuario;
                UsuarioSesion.Usuario.id = idUsuario;
                this.Hide();
                new MenuPrincipal().ShowDialog();
            }
            this.Close();
            

        }

        private void botonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login.LoginForm().ShowDialog();
            this.Close();
        }

       
    }
}