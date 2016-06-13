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
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Empresa
{
    public partial class EditarEmpresa : Form
    {
        private int idEmpresa;
        private int idContacto = 0;
        private int idUsuario = 0;
        private DBMapper mapper = new DBMapper();

        public EditarEmpresa(String idEmpresa)
        {
            InitializeComponent();
            this.idEmpresa = Convert.ToInt32(idEmpresa);
        }

        private void EditarEmpresa_Load(object sender, EventArgs e)
        {
            CargarRubros();
            CargarDatos();
        }

        private void CargarDatos()
        {
            Empresas empresa = mapper.ObtenerEmpresa(idEmpresa);
            Contacto contacto = mapper.ObtenerContacto(empresa.GetIdContacto());
            
            idUsuario = empresa.GetIdUsuario();
            idContacto = empresa.GetIdContacto();

            textBox_RazonSocial.Text = empresa.GetRazonSocial();
            textBox_Ciudad.Text = empresa.GetCiudad();
            textBox_CUIT.Text = empresa.GetCuit();
            textBox_NombreDeContacto.Text = empresa.GetNombreDeContacto();
            comboBox_Rubro.Text = empresa.GetRubro();
            textBox_FechaDeCreacion.Text = Convert.ToString(empresa.GetFechaDeCreacion());
            textBox_Mail.Text = contacto.GetMail();
            textBox_Telefono.Text = contacto.GetTelefono();
            checkBox_Habilitado.Checked = Convert.ToBoolean(mapper.SelectFromWhere("usr_activo", "Usuarios", "usr_id", empresa.GetIdUsuario()));
            textBox_Calle.Text = contacto.GetCalle();
            textBox_Numero.Text = contacto.GetNumeroCalle();
            textBox_Piso.Text = contacto.GetPiso();
            textBox_Departamento.Text = contacto.GetDepartamento();
            textBox_Localidad.Text = contacto.GetLocalidad();
            textBox_CodigoPostal.Text = contacto.GetCodigoPostal();
                              
            }

        private void CargarRubros()
        {
            string query = "SELECT rubro_id, rubro_desc_larga from NET_A_CERO.Rubros";

            SqlCommand cmd = new SqlCommand(query, ConnectionManager.Instance.getConnection());

            SqlDataAdapter data_adapter = new SqlDataAdapter(cmd);
            DataTable rubros = new DataTable();
            data_adapter.Fill(rubros);

            comboBox_Rubro.ValueMember = "rubro_id";
            comboBox_Rubro.DisplayMember = "rubro_desc_larga";
            comboBox_Rubro.DataSource = rubros;
            comboBox_Rubro.SelectedIndex = -1;
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            // Guarda en variables todos los campos de entrada
            String razonSocial = textBox_RazonSocial.Text;
            String ciudad = textBox_Ciudad.Text;
            String cuit = textBox_CUIT.Text;
            String nombreDeContacto = textBox_NombreDeContacto.Text;
            String rubro = comboBox_Rubro.Text;
            DateTime fechaDeCreacion;
            DateTime.TryParse(textBox_FechaDeCreacion.Text, out fechaDeCreacion);
            String mail = textBox_Mail.Text;
            String telefono = textBox_Telefono.Text;
            String calle = textBox_Calle.Text;
            String numero = textBox_Numero.Text;
            String piso = textBox_Piso.Text;
            String departamento = textBox_Departamento.Text;
            String localidad = textBox_Localidad.Text;
            String codigoPostal = textBox_CodigoPostal.Text;
            Boolean activo = checkBox_Habilitado.Checked;
            Boolean pudoModificar;

            // Update contacto
            Contacto contacto = new Contacto();
            try
            {
                contacto.setMail(mail);
                contacto.setTelefono(telefono);
                contacto.SetCalle(calle);
                contacto.SetNumeroCalle(numero);
                contacto.SetPiso(piso);
                contacto.SetDepartamento(departamento);
                contacto.SetLocalidad(localidad);
                contacto.SetCodigoPostal(codigoPostal);
                mapper.Modificar(idContacto, contacto);
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

            // Update empresa
            try
            {
                Empresas empresa = new Empresas();
                
                empresa.SetRazonSocial(razonSocial);
                empresa.SetCiudad(ciudad);
                empresa.SetCuit(cuit);
                empresa.SetNombreDeContacto(nombreDeContacto);
                empresa.SetRubro(rubro);
                empresa.SetFechaDeCreacion(fechaDeCreacion);
                empresa.SetIdContacto(idContacto);

                mapper.ActualizarEstadoUsuario(idUsuario, activo);

                pudoModificar = mapper.Modificar(idEmpresa, empresa);
                if (pudoModificar) MessageBox.Show("La empresa se modifico correctamente");
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
            catch (TelefonoYaExisteException exception)
            {
                MessageBox.Show("Telefono ya existe");
                return;
            }
            catch (CuitYaExisteException exception)
            {
                MessageBox.Show("Cuit ya existe");
                return;
            }
            catch (RazonSocialYaExisteException exception)
            {
                MessageBox.Show("RazonSocial ya existe");
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

        private void button_FechaDeCreacion_Click(object sender, EventArgs e)
        {
            monthCalendar_FechaDeCreacion.Visible = true;
        }

        private void monthCalendar_FechaDeCreacion_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaDeCreacion.Text = e.Start.ToShortDateString();
            monthCalendar_FechaDeCreacion.Visible = false;
        }
    }
}