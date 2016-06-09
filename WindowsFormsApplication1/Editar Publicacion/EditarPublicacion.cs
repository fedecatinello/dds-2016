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

namespace MercadoEnvio.Editar_Publicacion
{
    public partial class EditarPublicacion : Form
    {
        private Decimal idPublicacion;
        private Decimal idEstadoInicial;
        private String estadoInicial;
        private DBMapper comunicador = new DBMapper();

        public EditarPublicacion(String idPublicacion)
        {
            InitializeComponent();
            this.idPublicacion = Convert.ToDecimal(idPublicacion);
        }

        private void EditarPublicacion_Load(object sender, EventArgs e)
        {
            CargarTiposDePublicacion();
            CargarEstados();
            CargarRubros();
            CargarVisibilidades();
            CargarDatos();
        }

        private void CargarTiposDePublicacion()
        {
            comboBox_TiposDePublicacion.Items.Add("Compra Inmediata");
            comboBox_TiposDePublicacion.Items.Add("Subasta");
        }

        private void CargarEstados()
        {
            DataTable estados = new DataTable();
            estados.Columns.Add("estados");

            idEstadoInicial = (Decimal) comunicador.SelectFromWhere("publi_estado_id", "Publicaciones", "publi_id", idPublicacion);
            estadoInicial = (String)comunicador.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstadoInicial);

            if (estadoInicial == "Borrador") CargarSegunBorrador(estados);
            if (estadoInicial == "Publicada") CargarSegunPublicada(estados);
            if (estadoInicial == "Pausada") CargarSegunPausada(estados);
            if (estadoInicial == "Finalizada") CargarSegunFinalizada(estados);

            comboBox_Estado.DataSource = estados;
            comboBox_Estado.ValueMember = "estados";
        }

        private void CargarSegunBorrador(DataTable estados)
        {
            estados.Rows.Add("Borrador");
            estados.Rows.Add("Publicada");
        }

        private void CargarSegunPublicada(DataTable estados)
        {
            estados.Rows.Add("Publicada");
            estados.Rows.Add("Pausada");
            estados.Rows.Add("Finalizada");
            DesactivarCamposDeCaracteristicasComunes();
            DesactivarCamposDeCaracteristicasEspeciales();
            textBox_Descripcion.Enabled = true;
            textBox_Stock.Enabled = true;
        }

        private void CargarSegunPausada(DataTable estados)
        {
            estados.Rows.Add("Publicada");
            estados.Rows.Add("Pausada");
            estados.Rows.Add("Finalizada");
            DesactivarCamposDeCaracteristicasComunes();
            DesactivarCamposDeCaracteristicasEspeciales();
        }

        private void CargarSegunFinalizada(DataTable estados)
        {
            estados.Rows.Add("Finalizada");
            comboBox_Estado.Enabled = false;
            DesactivarCamposDeCaracteristicasComunes();
            DesactivarCamposDeCaracteristicasEspeciales();
        }

        private void DesactivarCamposDeCaracteristicasComunes()
        {
            textBox_Descripcion.Enabled = false;
            comboBox_Rubro.Enabled = false;
            comboBox_Visibilidad.Enabled = false;
            checkBox_Pregunta.Enabled = false;
        }

        private void DesactivarCamposDeCaracteristicasEspeciales()
        {
            textBox_Precio.Enabled = false;
            textBox_Stock.Enabled = false;
        }

        private void CargarRubros()
        {
            comboBox_Rubro.DataSource = comunicador.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBox_Rubro.ValueMember = "rubro_desc_larga";
        }

        private void CargarVisibilidades()
        {
            comboBox_Visibilidad.DataSource = comunicador.SelectDataTable("visib_desc", "NET_A_CERO.Visibilidad");
            comboBox_Visibilidad.ValueMember = "_visib_desc";
        }

        private void CargarDatos()
        {
            Publicacion publicacion = comunicador.ObtenerPublicacion(idPublicacion);
            textBox_Descripcion.Text = publicacion.GetDescripcion();
            textBox_Precio.Text = publicacion.GetPrecio();
            textBox_Stock.Text = publicacion.GetStock();
            comboBox_Rubro.SelectedValue = (String) comunicador.SelectFromWhere("rubro_desc_larga", "Rubros", "rubro_id", publicacion.GetIdRubro());
            comboBox_Visibilidad.SelectedValue = (String) comunicador.SelectFromWhere("visib_desc", "Visibilidad", "visib_id", publicacion.GetIdVisibilidad()); ;
            comboBox_TiposDePublicacion.SelectedValue = (String)comunicador.SelectFromWhere("publi_tipo", "Publicaciones", "publi_id", idPublicacion);
            comboBox_Estado.SelectedValue = (String) comunicador.SelectFromWhere("estado_desc", "Estado", "estado_id", publicacion.GetEstado());
            checkBox_Pregunta.Checked = Convert.ToBoolean(comunicador.SelectFromWhere("publi_preguntas", "Publicaciones", "publi_id", idPublicacion));
            
            //---------------------------------------CHEQUEAR ESTO--------------------------------------------------------//
            //checkBox_Habilitado.Checked = Convert.ToBoolean(comunicador.SelectFromWhere("habilitado", "Publicacion", "id", idPublicacion));
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            String tipoPublicacion = comboBox_TiposDePublicacion.Text;
            //Decimal idTipoDePublicacion = (Decimal)comunicador.SelectFromWhere("id", "TipoDePublicacion", "descripcion", tipo);
            String estado = comboBox_Estado.Text;
            Decimal idEstado = (Decimal)comunicador.SelectFromWhere("id", "Estado", "descripcion", estado);
            String descripcion = textBox_Descripcion.Text;
            String rubro = comboBox_Rubro.Text;
            String visibilidad = comboBox_Visibilidad.Text;
            Boolean pregunta = checkBox_Pregunta.Checked;
            String stock = textBox_Stock.Text;
            String precio = textBox_Precio.Text;
            Decimal idRubro = (Decimal) comunicador.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", rubro);
            Decimal idVisibilidad = (Decimal)comunicador.SelectFromWhere("visib_id", "Visibilidad", "visib_desc", visibilidad);
            //---------------------------------------CHEQUEAR ESTO--------------------------------------------------------//
            //Double duracion = Convert.ToDouble(comunicador.SelectFromWhere("duracion", "Visibilidad", "id", idVisibilidad));
            //Boolean habilitado = checkBox_Habilitado.Checked;
            DateTime fechaDeInicio;
            DateTime fechaDeVencimiento;

            if (estadoInicial == "Borrador")
            {
                //fechaDeInicio = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["DateKey"]); TP ANTERIOR
                fechaDeInicio = DateConfig.getInstance().getCurrentDate();
                fechaDeVencimiento = Convert.ToDateTime(Convert.ToString(Convert.ToDateTime(fechaDeInicio).AddDays(duracion)));
            }
            else
            {
                fechaDeInicio = Convert.ToDateTime(comunicador.SelectFromWhere("publi_fec_inicio", "Publicaciones", "publi_id", idPublicacion));
                fechaDeVencimiento = Convert.ToDateTime(comunicador.SelectFromWhere("publi_fec_vencimiento", "Publicaciones", "publi_id", idPublicacion));
            }

            // Update Publicacion
            try
            {
                Publicacion publicacion = new Publicacion();
                publicacion.SetTipo(tipoPublicacion);
                publicacion.SetEstado(idEstado);
                publicacion.SetDescripcion(descripcion);
                publicacion.SetFechaDeInicio(fechaDeInicio);
                publicacion.SetFechaDeVencimiento(fechaDeVencimiento);
                publicacion.SetPregunta(pregunta);
                publicacion.SetStock(stock);
                publicacion.SetPrecio(precio);
                publicacion.SetIdRubro(idRubro);
                publicacion.SetIdVisibilidad(idVisibilidad);
                //---------------VER ESTO---------------------------/
                //publicacion.SetHabilitado(habilitado);
                Boolean pudoModificar = comunicador.Modificar(idPublicacion, publicacion);
                if (pudoModificar) MessageBox.Show("La publicacion se modifico correctamente");
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
    }
}