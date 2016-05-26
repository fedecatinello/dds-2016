using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.Utils;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.Login
{
    public partial class CambiarContrasena : Form
    {
       
        public CambiarContrasena()
        {
            InitializeComponent();
        }

        private void botonContinuar_Click(object sender, EventArgs e)
        {
            if (textBoxContraseña.Text.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener por lo menos 8 caracteres");
                return;
            }

            if (textBoxContraseña.Text != textBoxRepetirContraseña.Text)
            {
                MessageBox.Show("La contraseña no se repite correctamente");
                return;
            }
            if (textBoxContraseña.Text == "")
            {
                MessageBox.Show("Debe ingresar una nueva contraseña");
                return;
            }

            if (textBoxRepetirContraseña.Text == "")
            {
                MessageBox.Show("Debe ingresar una nuevamenta la contraseña");
                return;
            }

            // Acualiza contraseña
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@username", UsuarioSesion.Usuario.nombre));
            parametros.Add(new SqlParameter("@pass", HashSha256.getHash(textBoxContraseña.Text)));
            String nuevaPass = "UPDATE NET_A_CERO.Usuarios SET usr_password = @pass WHERE username = @username";
            QueryBuilder.Instance.build(nuevaPass, parametros).ExecuteNonQuery();

            // Asigna rol
            parametros.Clear();
            parametros.Add(new SqlParameter("@username", UsuarioSesion.Usuario.nombre));

            String consultaRoles = "SELECT COUNT(rol_id) from NET_A_CERO.Usuarios_x_Rol WHERE (SELECT usr_id FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username) = usr_id";
            int cantidadDeRoles = (int)QueryBuilder.Instance.build(consultaRoles, parametros).ExecuteScalar();

            if (cantidadDeRoles > 1)
            {
                this.Hide();
                new ElegirRol().ShowDialog();
                this.Close();
            }
            else
            {
                parametros.Clear();
                parametros.Add(new SqlParameter("@username", UsuarioSesion.Usuario.nombre));
                String rolDeUsuario = "SELECT r.rol_nombre FROM NET_A_CERO.Roles r, NET_A_CERO.Usuarios_x_Rol ru, NET_A_CERO.Usuarios u WHERE r.rol_id = ru.rol_id and ru.usr_id = u.usr_id and u.usr_usuario = @username";
                String rolUser = (String)QueryBuilder.Instance.build(rolDeUsuario, parametros).ExecuteScalar();

                UsuarioSesion.Usuario.rol = rolUser;

                this.Hide();
                new MenuPrincipal().ShowDialog();
                this.Close();
            }

        }

        private void CambiarContrasena_Load(object sender, EventArgs e)
        {

        }


    }
}