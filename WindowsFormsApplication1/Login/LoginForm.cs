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
    public partial class LoginForm : Form
    {
        
        public LoginForm()
        {
            InitializeComponent();
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {
            
        }

        private void botonIngresar_Click(object sender, EventArgs e)
        {
            //Validamos que ingrese usuario y contraseña
            if (this.textBoxUsuario.Text == "")
            {
                MessageBox.Show("Debe ingresar un usuario");
                return;
            }

            if (this.textBoxContaseña.Text == "")
            {
                MessageBox.Show("Debe ingresar una contraseña");
                return;
            }


            // Nos fijamos si el usuario y contraseña existen y esta habilitado
            String query = "SELECT * FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username AND usr_password = @password AND usr_activo = 1";

            String usuario = this.textBoxUsuario.Text;
            // valida contraseña encriptada
            String contraseña = HashSha256.getHash(this.textBoxContaseña.Text);

            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@username", usuario));
            parametros.Add(new SqlParameter("@password", contraseña));

            SqlDataReader reader = QueryHelper.Instance.exec(query, parametros);

            //Chequea el ingreso
            if (QueryHelper.Instance.readFrom(reader))
            {
                MessageBox.Show("Bienvenido " + reader["usr_usuario"] + "!");

                UsuarioSesion.Usuario.nombre = (String)reader["usr_usuario"];
                UsuarioSesion.Usuario.id = (Int32)reader["usr_id"];

                // Usuario logueado correctamente (intentos fallidos = 0)
                parametros.Clear();
                parametros.Add(new SqlParameter("@username", usuario));
                String clearIntentosFallidos = "UPDATE NET_A_CERO.Usuarios SET usr_intentos = 0 WHERE usr_usuario = @username";
                QueryBuilder.Instance.build(clearIntentosFallidos, parametros).ExecuteNonQuery();

                // Se fija si es el primer inicio de sesion del usuario
                parametros.Clear();
                parametros.Add(new SqlParameter("@username", usuario));
                String sesion = "SELECT usr_password FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username"; 
                String primerInicio = (String)QueryBuilder.Instance.build(sesion, parametros).ExecuteScalar();
                if (primerInicio == "565339bc4d33d72817b583024112eb7f5cdf3e5eef0252d6ec1b9c9a94e12bb3") //El primer inicio la contraseña es fija (OK)
                {
                    this.Hide();
                    new CambiarContrasena().ShowDialog();
                    this.Close();
                }

                parametros.Clear();
                parametros.Add(new SqlParameter("@username", usuario));

                String consultaRoles = "SELECT COUNT(rol_id) FROM NET_A_CERO.Usuarios_x_Rol WHERE (SELECT usr_id FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username) = usr_id";
                int cantidadDeRoles = (int)QueryBuilder.Instance.build(consultaRoles, parametros).ExecuteScalar();

                if(cantidadDeRoles > 1)
                {
                    this.Hide();
                    new ElegirRol().ShowDialog();
                    this.Close();
                }
                else
                {
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@username", usuario));
                    String rolDeUsuario = "SELECT r.rol_nombre FROM NET_A_CERO.Roles r, NET_A_CERO.Usuarios_x_Rol ru, NET_A_CERO.Usuarios u WHERE r.rol_id = ru.rol_id AND ru.usr_id = u.usr_id AND u.usr_usuario = @username";
                    String rolUser = (String)QueryBuilder.Instance.build(rolDeUsuario, parametros).ExecuteScalar();

                    UsuarioSesion.Usuario.rol = rolUser;
                    if(UsuarioSesion.Usuario.rol == null)
                    {
                        MessageBox.Show("Usted no tiene roles para iniciar sesion");
                        return;
                    }
                  //  MessageBox.Show("Rol: " + UsuarioSesion.Usuario.rol);

                    this.Hide();
                    new MenuPrincipal().ShowDialog();
                    this.Close();
                }

            }
            else
            {
                // Se fija si el usuario era correcto
                parametros.Clear();
                parametros.Add(new SqlParameter("@username", usuario));
                String buscaUsuario = "SELECT * FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username";
                SqlDataReader lector = QueryBuilder.Instance.build(buscaUsuario, parametros).ExecuteReader();

                if (lector.Read())
                {

                    // Se fija si el usuario esta inhabilitado
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@username", usuario));
                    parametros.Add(new SqlParameter("@password", contraseña));
                    String estaDeshabilitado = "SELECT * FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username AND usr_activo = 0";

                    SqlDataReader leeDeshabilitado = QueryBuilder.Instance.build(estaDeshabilitado, parametros).ExecuteReader();

                    if (leeDeshabilitado.Read())
                    {
                        MessageBox.Show("El usuario esta deshabilitado");
                        return;
                    }

                    // Suma un fallido
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@username", usuario));
                    String sumaFallido = "UPDATE NET_A_CERO.Usuarios SET usr_intentos = usr_intentos + 1 WHERE usr_usuario = @username";
                    QueryBuilder.Instance.build(sumaFallido, parametros).ExecuteNonQuery();


                    // Si es el tercer fallido se deshabilita al usuario
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@username", usuario));
                    String cantidadFallidos = "SELECT login_fallidos FROM NET_A_CERO.Usuarios WHERE usr_usuario = @username";
                    int intentosFallidos = (int)QueryBuilder.Instance.build(cantidadFallidos, parametros).ExecuteScalar();

                    if (intentosFallidos == 3)
                    {
                        parametros.Clear();
                        parametros.Add(new SqlParameter("@username", usuario));
                        String deshabilitar = "UPDATE NET_A_CERO.Usuarios SET usr_activo = 0 WHERE usr_usuario = @username";
                        QueryBuilder.Instance.build(deshabilitar, parametros).ExecuteNonQuery();
                    }
                    MessageBox.Show("Contraseña incorrecta." + '\n' + "La contrseña distingue mayusculas y minusculas." + '\n' + "Fallidos del usuario: " + intentosFallidos);
                }
                else 
                {
                    MessageBox.Show("El usuario no existe");
                }
       
                
            }
        }

        private void textBoxContaseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void botonRegistrarse_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Registro_de_Usuario.RegistrarUsuario().ShowDialog();
            this.Close();
        }
    }
}