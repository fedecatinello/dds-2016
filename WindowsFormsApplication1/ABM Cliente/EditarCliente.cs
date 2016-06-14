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
using MercadoEnvio.ABM_Cliente;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Cliente
{
    public partial class EditarCliente : Form
    {
        private int idCliente = 0;
        private int idContacto = 0;
        private int idUsuario = 0;
        private DBMapper mapper = new DBMapper();
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        
        public EditarCliente(String idCliente)
        {
            InitializeComponent();
            this.idCliente = Convert.ToInt32(idCliente);
        }

        private void EditarCliente_Load(object sender, EventArgs e)
        {
            CargarTipoDeDocumentos(); 
            CargarDatos();
        }

        private void CargarTipoDeDocumentos()
        {
            comboBox_TipoDeDocumento.Items.Add("DNI - Documento Nacional de Identidad");
            comboBox_TipoDeDocumento.Items.Add("Pasaporte");
            comboBox_TipoDeDocumento.Items.Add("LC - Libreta Civica");
            comboBox_TipoDeDocumento.Items.Add("LE - Libreta de Enrolamiento");
        }

        private void CargarDatos()
        {
            Clientes cliente = mapper.ObtenerCliente(idCliente);
            Contacto contacto = mapper.ObtenerContacto(cliente.GetIdContacto());

            //Me guardo el id contacto y usuario
            idUsuario = cliente.GetIdUsuario();
            idContacto = cliente.GetIdContacto();

            textBox_Nombre.Text = cliente.GetNombre();
            textBox_Apellido.Text = cliente.GetApellido();
            textBox_NumeroDeDoc.Text = cliente.GetNumeroDeDocumento();
            comboBox_TipoDeDocumento.Text = cliente.GetTipoDeDocumento();
            textBox_FechaDeNacimiento.Text = Convert.ToString(cliente.GetFechaDeNacimiento());
            textBox_Mail.Text = contacto.GetMail();
            textBox_Telefono.Text = contacto.GetTelefono();
            textBox_Calle.Text = contacto.GetCalle();
            textBox_Numero.Text = contacto.GetNumeroCalle();
            textBox_Piso.Text = contacto.GetPiso();
            textBox_Departamento.Text = contacto.GetDepartamento();
            textBox_Localidad.Text = contacto.GetLocalidad();
            textBox_CodigoPostal.Text = contacto.GetCodigoPostal();

            checkBox_Habilitado.Checked = Convert.ToBoolean(mapper.SelectFromWhere("usr_activo", "Usuarios", "usr_id", cliente.GetIdUsuario()));
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
            Boolean activo = checkBox_Habilitado.Checked; //La variable activo que esta en el checkbox es para saber si esta habilitado a nivel usuario

            Boolean pudoModificar;

            // Update contacto
            try
            {
                Contacto contacto = new Contacto();
                
                contacto.setMail(mail);
                contacto.setTelefono(telefono);
                contacto.SetCalle(calle);
                contacto.SetNumeroCalle(numeroCalle);
                contacto.SetPiso(piso);
                contacto.SetDepartamento(departamento);
                contacto.SetCodigoPostal(codigoPostal);
                contacto.SetLocalidad(localidad);
                mapper.Modificar(idContacto, contacto);

            }
            catch (CampoVacioException exception)
            {
                MessageBox.Show("Falta completar campo: " + exception.Message);
                return;
            }
            catch (FormatoInvalidoException exception)
            {
                MessageBox.Show("Los datos fueron mal ingresados en: " + exception.Message);
                return;
            }

            // Update cliente
            try
            {
                Clientes cliente = new Clientes();
                
                cliente.SetNombre(nombre);
                cliente.SetApellido(apellido);
                cliente.SetNumeroDeDocumento(numeroDeDocumento);
                cliente.SetTipoDeDocumento(tipoDeDocumento);
                cliente.SetFechaDeNacimiento(fechaDeNacimiento);
                cliente.SetActivo(true); 

                /** La fecha de alta no se actualiza en la DB **/

                mapper.ActualizarEstadoUsuario(idUsuario, activo);

                pudoModificar = mapper.Modificar(idCliente, cliente);
                if (pudoModificar) MessageBox.Show("El cliente se modifico correctamente");
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

            this.Close();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_FechaDeNacimiento_Click(object sender, EventArgs e)
        {
            this.monthCalendar_FechaDeNacimiento.Visible = true;
        }

        private void monthCalendar_FechaDeNacimiento_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaDeNacimiento.Text = e.Start.ToShortDateString();
            monthCalendar_FechaDeNacimiento.Visible = false;
        }
    }
}