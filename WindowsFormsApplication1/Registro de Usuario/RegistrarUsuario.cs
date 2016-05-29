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
        private DBCommunicator comunicador = new DBCommunicator();

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
            command = QueryBuilder.Instance.build("SELECT DISTINCT nombre FROM NET_A_CERO.Roles WHERE rol_activo = 1 AND rol_nombre != 'Administrador'", parametros);
            adapter.SelectCommand = command;
            adapter.Fill(roles, "Rol");
            comboBoxRol.DataSource = roles.Tables[0].DefaultView;
            comboBoxRol.ValueMember = "nombre";
        }

        private void botonSiguiente_Click(object sender, EventArgs e)
        {

            String rolElegido = this.comboBoxRol.Text;
            String usuario = this.textBoxUsuario.Text;
            String contraseña = this.textBoxPass.Text;
            String repetirContraseña = this.textBoxPass2.Text;

            if (usuario == "")
            {
                MessageBox.Show("Debe completarse el campo Usuario");
                return;
            }

            if (contraseña == "")
            {
                MessageBox.Show("Debe completarse el campo Contraseña");
                return;
            }
            if (repetirContraseña == "")
            {
                MessageBox.Show("Debe completarse el campo Repetir contraseña");
                return;
            }

            if (rolElegido == "")
            {
                MessageBox.Show("Debe seleccionarse un rol");
                return;
            }

            if (contraseña.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener por lo menos 8 caracteres");
                return;
            }

            if (textBoxPass.Text != textBoxPass2.Text)
            {
                MessageBox.Show("La contraseña no se repite correctamente");
                return;
            }

            parametros.Clear();
            parametros.Add(new SqlParameter("@username", usuario));

            // Buscamos si el username ya se encuentra registrado
            String consulta = "SELECT id FROM LOS_SUPER_AMIGOS.Usuario WHERE username = @username";

            SqlDataReader reader = QueryBuilder.Instance.build(consulta, parametros).ExecuteReader();

            if (reader.Read())
            {
                MessageBox.Show("Ya existe un usuario con ese nombre");
                return;
            }

            if (rolElegido == "Clientes")
            {
                this.Hide();
                new ABM_Cliente.AgregarCliente(usuario,contraseña).ShowDialog();

                if (UsuarioSesion.Usuario.rol != "Administrador")
                {
                    UsuarioSesion.Usuario.rol = "Clientes";
                    UsuarioSesion.Usuario.nombre = usuario;

                    String idUsuario = "select top 1 id" 
                                + " from NET_A_CERO.Usuarios"
                                + " order by id DESC";
                    parametros.Clear();
                    Decimal idC = (Decimal)QueryBuilder.Instance.build(idUsuario,parametros).ExecuteScalar();

                    UsuarioSesion.Usuario.id = idC;
                }
   
            }
            else if (rolElegido == "Empresa")
            {
                new ABM_Empresa.AgregarEmpresa(usuario, contraseña).ShowDialog();

            }
            else
            {
                Decimal idUsuario = comunicador.CrearUsuarioConValores(usuario, contraseña);
                comunicador.AsignarRolAUsuario(idUsuario, rolElegido);
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