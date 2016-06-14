using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.Modelo;
using MercadoEnvio.Exceptions;
using MercadoEnvio.Utils;
using MercadoEnvio.DataProvider;
namespace MercadoEnvio.ABM_Cliente
{

    public partial class AgregarCliente : Form
    {

        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private String username;
        private String contrasena;
        private DBMapper mapper = new DBMapper();
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
                this.idContacto = mapper.CrearContacto(contacto);
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
                cliente.SetActivo(true);


                idCliente = mapper.CrearCliente(cliente);
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
                Boolean seCreoBien = mapper.AsignarUsuarioACliente(idCliente, idUsuario);
                if (seCreoBien) MessageBox.Show("Se creo el usuario correctamente");
            }

            mapper.AsignarRolAUsuario(this.idUsuario, "Cliente");

            VolverAlMenuPrincipal();
        }

        private int CrearUsuario()
        {
            if (username == "clienteCreadoPorAdmin") 
            {
                return mapper.CrearUsuario(); //Se crean con los parametros default
            }
            else
            {
                return mapper.CrearUsuarioConValores(username, contrasena); //Si es por registro de usuario, segun los parametros dados
            }
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Nombre.Text = "";
            textBox_Apellido.Text = "";
            comboBox_TipoDeDocumento.SelectedIndex = -1;
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

            if (UsuarioSesion.Usuario.rol != "Administrativo")
            {
                this.Hide();
                new Registro_de_Usuario.RegistrarUsuario().ShowDialog();
                this.Close();
            }
            else
            {
                VolverAlMenuPrincipal();
            }
            
        }

        private void VolverAlMenuPrincipal()
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