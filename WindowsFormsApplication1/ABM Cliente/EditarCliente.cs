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
using MercadoEnvio.ABM_Cliente;

namespace MercadoEnvio.ABM_Cliente
{
    public partial class EditarCliente : Form
    {
        private Decimal idCliente;
        private Decimal idContacto;
        private DBCommunicator comunicador = new DBCommunicator();

        public EditarCliente(String idCliente)
        {
            InitializeComponent();
            this.idCliente = Convert.ToDecimal(idCliente);
        }

        private void EditarCliente_Load(object sender, EventArgs e)
        {
            CargarTipoDeDocumentos(); 
            CargarDatos();
        }

        private void CargarTipoDeDocumentos() //Chequear como hacer para que se sincronice con el de agregarCliente
        {
            comboBox_TipoDeDocumento.Items.Add("DNI - Documento Nacional de Identidad");
            comboBox_TipoDeDocumento.Items.Add("Pasaporte");
            comboBox_TipoDeDocumento.Items.Add("LC - Libreta Civica");
            comboBox_TipoDeDocumento.Items.Add("LE - Libreta de Enrolamiento");
        }

        private void CargarDatos()
        {
            Clientes cliente = comunicador.ObtenerCliente(idCliente);
            Contacto contacto = comunicador.ObtenerContacto(idContacto);

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

            checkBox_Habilitado.Checked = Convert.ToBoolean(comunicador.SelectFromWhere("usr_activo", "Usuarios", "id", idCliente));
        }

/*-------------------NO SE USA PORQUE LO IMPLEMENTAMOS DE OTRA MANERA------------------------------*/

/*      private void CargarTipoDeDocumento(Decimal idTipoDeDocumento)
        {
            comboBox_TipoDeDocumento.SelectedValue = (String) comunicador.SelectFromWhere("nombre", "TipoDeDocumento", "id", idTipoDeDocumento);
        }

        private void CargarDireccion(Decimal idDireccion)
        {
            Contacto contacto = comunicador.ObtenerDireccion(idDireccion);
            textBox_Calle.Text = contacto.GetCalle();
            textBox_Numero.Text = contacto.GetNumero();
            textBox_Piso.Text = contacto.GetPiso();
            textBox_Departamento.Text = contacto.GetDepartamento();
            textBox_CodigoPostal.Text = contacto.GetCodigoPostal();
            textBox_Localidad.Text = contacto.GetLocalidad();
        }
        */

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
            Boolean activo = checkBox_Habilitado.Checked;

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
                comunicador.Modificar(idContacto, contacto);
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

            // Update cliente
            try
            {
                Clientes cliente = new Clientes();
                Usuarios usuario = new Usuarios();

                cliente.SetNombre(nombre);
                cliente.SetApellido(apellido);
                cliente.SetNumeroDeDocumento(numeroDeDocumento);
                cliente.SetTipoDeDocumento(tipoDeDocumento);
                cliente.SetFechaDeNacimiento(fechaDeNacimiento);
                usuario.SetActivo(activo);

                pudoModificar = comunicador.Modificar(idCliente, cliente);
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