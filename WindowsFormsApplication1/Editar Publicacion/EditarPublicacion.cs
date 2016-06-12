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

namespace MercadoEnvio.Editar_Publicacion
{
    public partial class EditarPublicacion : Form
    {
        private int idPublicacion;
        private int idEstadoInicial;
        private String estadoInicial;
        private DBMapper mapper = new DBMapper();

        public EditarPublicacion(String idPublicacion)
        {
            InitializeComponent();
            this.idPublicacion = Convert.ToInt32(idPublicacion);
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
            comboBox_TiposDePublicacion.Items.Add("Compra inmediata");
            comboBox_TiposDePublicacion.Items.Add("Subasta");
            comboBox_TiposDePublicacion.SelectedIndex = -1;
        }

        private void CargarEstados()
        {
            DataTable estados = new DataTable();
            estados.Columns.Add("estados");


            idEstadoInicial = Convert.ToInt32(mapper.SelectFromWhere("publi_estado_id", "Publicaciones", "publi_id", idPublicacion));
            estadoInicial = Convert.ToString(mapper.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstadoInicial));

            if (estadoInicial == "Borrador") CargarSegunBorrador(estados);
            if (estadoInicial == "Activa") CargarSegunPublicada(estados);
            if (estadoInicial == "Pausada") CargarSegunPausada(estados);
            if (estadoInicial == "Finalizada") CargarSegunFinalizada(estados);
          
            comboBox_Estado.DataSource = estados;
            comboBox_Estado.ValueMember = "estados";
            comboBox_Estado.SelectedIndex = -1;
         }

        private void CargarSegunBorrador(DataTable estados)
        {
            estados.Rows.Add("Borrador");
            estados.Rows.Add("Activa");
        }

        private void CargarSegunPublicada(DataTable estados)
        {
            estados.Rows.Add("Activa");
            estados.Rows.Add("Pausada");
            estados.Rows.Add("Finalizada");
            DesactivarCamposDeCaracteristicasComunes();
            DesactivarCamposDeCaracteristicasEspeciales();
            textBox_Descripcion.Enabled = true;
            textBox_Stock.Enabled = true;
        }

        private void CargarSegunPausada(DataTable estados)
        {
            estados.Rows.Add("Activa");
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
            comboBox_Rubro.DataSource = mapper.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBox_Rubro.ValueMember = "rubro_desc_larga";
            comboBox_Rubro.SelectedIndex = -1;
        }

        private void CargarVisibilidades()
        {
            comboBox_Visibilidad.DataSource = mapper.SelectDataTable("visib_desc", "NET_A_CERO.Visibilidad");
            comboBox_Visibilidad.ValueMember = "visib_desc";
            comboBox_Visibilidad.SelectedIndex = -1;
        }

        private void CargarDatos()
        {
            
            Publicacion publicacion = mapper.ObtenerPublicacion(idPublicacion);
            textBox_Descripcion.Text = publicacion.GetDescripcion();
            textBox_Precio.Text = publicacion.GetPrecio();
            textBox_Stock.Text = publicacion.GetStock();
            Decimal rubroId = Convert.ToInt32(mapper.SelectFromWhere("rubro_id", "Rubro_x_Publicacion", "publi_id", idPublicacion));
            comboBox_Rubro.SelectedValue = Convert.ToString(mapper.SelectFromWhere("rubro_desc_larga", "Rubros", "rubro_id", rubroId));
            comboBox_Visibilidad.SelectedValue = Convert.ToString(mapper.SelectFromWhere("visib_desc", "Visibilidad", "visib_id", publicacion.GetIdVisibilidad()));

            comboBox_TiposDePublicacion.Text = publicacion.GetTipo();

            comboBox_Estado.SelectedValue = Convert.ToString(mapper.SelectFromWhere("estado_desc", "Estado", "estado_id", publicacion.GetEstado()));
            checkBox_Pregunta.Checked = Convert.ToBoolean(mapper.SelectFromWhere("publi_preguntas", "Publicaciones", "publi_id", idPublicacion));
  
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            String tipoPublicacion = comboBox_TiposDePublicacion.Text;
            String estado = comboBox_Estado.Text;
            Decimal idEstado = Convert.ToInt32(mapper.SelectFromWhere("estado_id", "Estado", "estado_desc", estado));
            String descripcion = textBox_Descripcion.Text;
            String rubro = comboBox_Rubro.Text;
            String visibilidad = comboBox_Visibilidad.Text;
            Boolean pregunta = checkBox_Pregunta.Checked;
            String stock = textBox_Stock.Text;
            String precio = textBox_Precio.Text;
            Decimal idRubro = Convert.ToInt32(mapper.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", rubro));
            Decimal idVisibilidad = Convert.ToInt32(mapper.SelectFromWhere("visib_id", "Visibilidad", "visib_desc", visibilidad));
            //-------------------------CHEQUEAR DURACION-------------------------------------//
            //Double duracion = Convert.ToDouble(comunicador.SelectFromWhere("duracion", "Visibilidad", "id", idVisibilidad));
            Double duracion = 30;
            DateTime fechaDeInicio;
            DateTime fechaDeVencimiento;

            if (estadoInicial == "Borrador")
            {
                fechaDeInicio = DateConfig.getInstance().getCurrentDate();
                fechaDeVencimiento = Convert.ToDateTime(Convert.ToString(Convert.ToDateTime(fechaDeInicio).AddDays(duracion)));
            }
            else
            {
                fechaDeInicio = Convert.ToDateTime(mapper.SelectFromWhere("publi_fec_inicio", "Publicaciones", "publi_id", idPublicacion));
                fechaDeVencimiento = Convert.ToDateTime(mapper.SelectFromWhere("publi_fec_vencimiento", "Publicaciones", "publi_id", idPublicacion));
            }

            // Update Publicacion
            try
            {
                Publicacion publicacion = new Publicacion();
                publicacion.SetTipo(tipoPublicacion);
                publicacion.SetDescripcion(descripcion);
                publicacion.SetStock(stock);
                publicacion.SetFechaDeVencimiento(fechaDeVencimiento);
                publicacion.SetFechaDeInicio(fechaDeInicio);
                publicacion.SetPrecio(precio);
                publicacion.SetCostoPagado(true);
                publicacion.SetPregunta(pregunta);
                publicacion.SetIdVisibilidad(idVisibilidad);
                publicacion.SetEstado(idEstado);
                publicacion.SetIdRubro(idRubro);
                
                Boolean pudoModificar = mapper.Modificar(idPublicacion, publicacion);
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
            catch (IngresePrecioEnteroException exception)
            {
                MessageBox.Show("Ingrese un precio entero");
                return;
            }

            this.Close();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            CargarEstados();
            CargarDatos();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}