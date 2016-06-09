using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.Objetos;
using MercadoEnvio.Exceptions;
using MercadoEnvio.Utils;
using MercadoEnvio.DataProvider;
namespace MercadoEnvio.ABM_Cliente
{

    public partial class AgregarCliente : Form
    {
        private String query;
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private SqlParameter parametroOutput;
        private SqlCommand command;
        private String username;
        private String contrasena;
        private DBMapper comunicador = new DBMapper();
        private int idContacto;
        private int idUsuario;
        private int idCliente;
       

        public AgregarCliente(String username, String contrasena)
        {
            InitializeComponent();
            this.username = username;
            this.contrasena = contrasena;
            this.idContacto = 0;
            this.idUsuario = 0;
        }

        private void AgregarCliente_Load(object sender, EventArgs e)
        {
            CargarTipoDeDocumentos();
        }

        public void CargarTipoDeDocumentos()
        {
            comboBox_TipoDeDocumento.Items.Add("DNI - Documento Nacional de Identidad");
            comboBox_TipoDeDocumento.Items.Add("Pasaporte");
            comboBox_TipoDeDocumento.Items.Add("LC - Libreta Civica");
            comboBox_TipoDeDocumento.Items.Add("LE - Libreta de Enrolamiento");
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            // Guarda en variables todos los campos de entrada
            String nombre = textBox_Nombre.Text;
            String apellido = textBox_Apellido.Text;
            String tipoDeDocumento = comboBox_TipoDeDocumento.Text;
            String numeroDeDocumento = textBox_NumeroDeDoc.Text;
            DateTime fechaDeNacimiento;
            DateTime.TryParse(textBox_FechaDeNacimiento.Text, out fechaDeNacimiento);
            String mail = textBox_Mail.Text;
            String telefono = textBox_Telefono.Text;
            String calle = textBox_Calle.Text;
            String numeroCalle = textBox_Numero.Text;
            String piso = textBox_Piso.Text;
            String departamento = textBox_Departamento.Text;
            String codigoPostal = textBox_CodigoPostal.Text;
            String localidad = textBox_Localidad.Text;

            // Crea una contacto y se guarda su id
            Contacto contacto = new Contacto();
            try
            {
                contacto.setMail(mail);
                contacto.setTelefono(telefono);
                contacto.SetCalle(calle);
                contacto.SetNumeroCalle(numeroCalle);
                contacto.SetPiso(piso);
                contacto.SetDepartamento(departamento);
                contacto.SetCodigoPostal(codigoPostal);
                contacto.SetLocalidad(localidad);
            }
            catch (CampoVacioException exception)
            {
                MessageBox.Show("Falta completar campo: " + exception.Message);
                return;
            }
            catch (FormatoInvalidoException exception)
            {
                MessageBox.Show("Datos mal ingresados en: " + exception.Message);
                return;
            }
            // Controla que no se haya creado ya el contacto
            if (this.idContacto == 0)
            {
                this.idContacto = comunicador.CrearContacto(contacto);
            } 

            // Crear cliente
            try
            {
                Clientes cliente = new Clientes();
                Usuarios usuario = new Usuarios();

                cliente.SetNombre(nombre);
                cliente.SetApellido(apellido);
                cliente.SetNumeroDeDocumento(numeroDeDocumento);
                cliente.SetTipoDeDocumento(tipoDeDocumento);
                cliente.SetFechaDeNacimiento(fechaDeNacimiento);
                cliente.SetFechaDeAlta(DateConfig.getInstance().getCurrentDate());
                cliente.SetIdUsuario(idUsuario);
                cliente.SetIdContacto(idContacto);
                //usuario.SetActivo(true);


                idCliente = comunicador.CrearCliente(cliente);
                if (idCliente > 0) MessageBox.Show("Se agrego el cliente correctamente");
            }
            catch (CampoVacioException exception)
            {
                MessageBox.Show("Falta completar campo: " + exception.Message);
                return;
            }
            catch (FormatoInvalidoException exception)
            {
                MessageBox.Show("Datos mal ingresados en: " + exception.Message);
                return;
            }
            catch (ClienteYaExisteException exception)
            {
                MessageBox.Show("El documento ya existe");
                return;
            }
            catch (TelefonoYaExisteException exception)
            {
                MessageBox.Show("El telefono ya existe");
                return;
            }
            catch (FechaPasadaException exception)
            {
                MessageBox.Show("Fecha no valida");
                return;
            }

            // Si el cliente lo crea el admin, crea un nuevo usuario predeterminado. Si lo crea un nuevo registro de usuario, usa el que viene por parametro
            if (idUsuario == 0)
            {
                idUsuario = CrearUsuario();
                idUsuario = comunicador.CrearUsuarioConValores(username, contrasena);
                Boolean seCreoBien = comunicador.AsignarUsuarioACliente(idCliente, idUsuario);
                if (seCreoBien) MessageBox.Show("Se creo el usuario correctamente");
            }

            /*
             ------------- SOLO LOS ADMINISTRADORES PUEDEN CREAR USUARIOS-----------------
             if (UsuarioSesion.Usuario.rol != "Administrador")
            {
                UsuarioSesion.Usuario.rol = "Clientes";
                UsuarioSesion.Usuario.nombre = username;
                UsuarioSesion.Usuario.id = idUsuario;
            }
             */

            comunicador.AsignarRolAUsuario(this.idUsuario, "Cliente");

            VolverAlMenuPrincial();
        }

        public Boolean AsignarRolAUsuario(Decimal idUsuario, String rol)
        {
            Decimal idRol = Convert.ToDecimal(this.SelectFromWhere("id", "Rol", "nombre", rol));
            query = "NET_A_CERO.agregar_rol_a_usuario";
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario_id", idUsuario));
            parametros.Add(new SqlParameter("@rol_id", idRol));
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Object SelectFromWhere(String que, String deDonde, String param1, String param2)
        {
            query = "SELECT " + que + " FROM NET_A_CERO." + deDonde + " WHERE " + param1 + " = @" + param1;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + param1, param2));
            return QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
        }

        public Boolean AsignarUsuarioACliente(Decimal idCliente, Decimal idUsuario)
        {
            query = "UPDATE NET_A_CERO.Clientes SET usr_id = @idUsuario WHERE cli_id = @idCliente";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idUsuario", idUsuario));
            parametros.Add(new SqlParameter("@idCliente", idCliente));
            command = QueryBuilder.Instance.build(query, parametros);
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public int CrearUsuarioConValores(String username, String password)
        {
            query = "NET_A_CERO.pr_crear_usuario_con_valores";
            parametros.Clear();
            parametroOutput = new SqlParameter("@usuario_id", SqlDbType.Int);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(new SqlParameter("@username", username));
            parametros.Add(new SqlParameter("@password", HashSha256.getHash(password)));
            parametros.Add(new SqlParameter("@is_admin", "0"));
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (int)parametroOutput.Value;
        }

        private int CrearUsuario()
        {
            /*
             ------------- SOLO LOS ADMINISTRADORES PUEDEN CREAR USUARIOS-----------------
            if (username == "clienteCreadoPorAdmin")
            {
                return comunicador.CrearUsuario();
            }
            else
            {*/
                return comunicador.CrearUsuarioConValores(username, contrasena);
            //}
        }
       
        private   int CrearContacto(Contacto contacto)
        {
            return this.Crear(contacto);
        }

        private int Crear(Comunicable objeto)
        {
            query = objeto.GetQueryCrear();
            parametros.Clear();
            parametros = objeto.GetParametros();
            parametroOutput = new SqlParameter("@id", SqlDbType.Int);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (int)parametroOutput.Value;
        }

        private int CrearCliente(Clientes cliente)
        {

            if (!pasoControlDeRegistro(cliente.GetTipoDeDocumento(), cliente.GetNumeroDeDocumento()))
                throw new ClienteYaExisteException();

            return this.Crear(cliente);

        }
        private bool pasoControlDeRegistro(String tipoDeDocumento, String numeroDeDocumento)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Clientes WHERE cli_tipo_dni = @tipoDeDocumento AND cli_dni = @numeroDeDocumento";
            parametros.Clear();
            parametros.Add(new SqlParameter("@tipoDeDocumento", tipoDeDocumento));
            parametros.Add(new SqlParameter("@numeroDeDocumento", Convert.ToDecimal(numeroDeDocumento)));
            return ControlDeUnicidad(query, parametros);
        }

        private bool ControlDeUnicidad(String query, IList<SqlParameter> parametros)
        {
            int cantidad = (int)QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
            if (cantidad > 0)
            {
                return false;
            }
            return true;
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Nombre.Text = "";
            textBox_Apellido.Text = "";
            comboBox_TipoDeDocumento.SelectedIndex = 0;
            textBox_NumeroDeDoc.Text = "";
            textBox_FechaDeNacimiento.Text = "";
            textBox_Mail.Text = "";
            textBox_Telefono.Text = "";
            textBox_Calle.Text = "";
            textBox_Numero.Text = "";
            textBox_Piso.Text = "";
            textBox_Departamento.Text = "";
            textBox_CodigoPostal.Text = "";
            textBox_Localidad.Text = "";
        }      

        private void button_Cancelar_Click(object sender, EventArgs e)
        {

            if (UsuarioSesion.Usuario.rol != "Administrador")
            {
                this.Hide();
                new Registro_de_Usuario.RegistrarUsuario().ShowDialog();
                this.Close();
            }
            else
            {
                VolverAlMenuPrincial();
            }
            
        }

        private void VolverAlMenuPrincial()
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void button_FechaDeNacimiento_Click(object sender, EventArgs e)
        {
            monthCalendar_FechaDeNacimiento.Visible = true;
        }

        private void monthCalendar_FechaDeNacimiento_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaDeNacimiento.Text = e.Start.ToShortDateString();
            monthCalendar_FechaDeNacimiento.Visible = false;
        }
    }
}