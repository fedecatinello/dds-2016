using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.Exceptions;
using MercadoEnvio.Modelo;

namespace MercadoEnvio.Generar_Publicacion
{
    public partial class GenerarPublicacion : Form
    {
        private DBMapper comunicador = new DBMapper();

        public GenerarPublicacion()
        {
            InitializeComponent();
        }

        private void GenerarPublicacion_Load(object sender, EventArgs e)
        {
            CargarTiposDePublicacion();
            CargarEstados();
            CargarRubros();
            CargarVisibilidades();
        }

        private void CargarTiposDePublicacion()
        {
            comboBox_TiposDePublicacion.Items.Add("Compra inmediata");
            comboBox_TiposDePublicacion.Items.Add("Subasta");
        }

        private void CargarEstados()
        {
            DataTable estados = new DataTable();
            estados.Columns.Add("estados");
            estados.Rows.Add("Borrador");
            estados.Rows.Add("Activa");
            comboBox_Estado.DataSource = estados;
            comboBox_Estado.ValueMember = "estados";
        }

        private void CargarRubros()
        {
            comboBox_Rubro.DataSource = comunicador.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBox_Rubro.ValueMember = "rubro_desc_larga";
        }

        private void CargarVisibilidades()
        {
            comboBox_Visibilidad.DataSource = comunicador.SelectDataTable("visib_desc", "NET_A_CERO.Visibilidad");
            comboBox_Visibilidad.ValueMember = "visib_desc";
        }

        private void button_generar_Click(object sender, EventArgs e)
        {
            String tipo = comboBox_TiposDePublicacion.Text;
            String estado = comboBox_Estado.Text;
            String descripcion = textBox_Descripcion.Text;
            //DateTime fechaDeInicio = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["DateKey"]);
            DateTime fechaDeInicio = DateConfig.getInstance().getCurrentDate();
            String rubro = comboBox_Rubro.Text;
            String visibilidadDescripcion = comboBox_Visibilidad.Text;
            Boolean pregunta = radioButton_Pregunta.Checked;
            String stock = textBox_Stock.Text;
            String precio = textBox_Precio.Text;
            String tipoPublicacion = comboBox_TiposDePublicacion.Text;

            Decimal idRubro = Convert.ToDecimal(comunicador.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", rubro));
            Decimal idEstado = Convert.ToDecimal(comunicador.SelectFromWhere("estado_id", "Estado", "estado_desc", estado));
            Decimal idVisibilidad = Convert.ToDecimal(comunicador.SelectFromWhere("visib_id", "Visibilidad", "visib_desc", visibilidadDescripcion));
            
            
            //-------------------------CHEQUEAR DURACION-------------------------------------
            //Double duracion = Convert.ToDouble(comunicador.SelectFromWhere("duracion", "Visibilidad", "descripcion", visibilidadDescripcion)); 
            Double duracion = 30;
            DateTime fechaDeVencimiento = Convert.ToDateTime(Convert.ToString(Convert.ToDateTime(fechaDeInicio).AddDays(duracion)));

            // Insert Publicacion
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
                publicacion.SetIdUsuario(UsuarioSesion.Usuario.id);
                publicacion.SetIdVisibilidad(idVisibilidad);
                publicacion.SetEstado(idEstado);
                publicacion.SetIdRubro(idRubro);
                //publicacion.SetHabilitado(true);
                Decimal idPublicacion = comunicador.CrearPublicacion(publicacion);
                if (idPublicacion > 0) MessageBox.Show("Se agrego la publicacion correctamente");
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
            
            VolverAlMenuPrincipal();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Descripcion.Text = "";
            textBox_Precio.Text = "";
            textBox_Stock.Text = "";
            comboBox_Rubro.SelectedIndex = 0;
            comboBox_TiposDePublicacion.SelectedIndex = 0;
            comboBox_Estado.SelectedIndex = 0;
            comboBox_Visibilidad.SelectedIndex = 0;
            radioButton_Pregunta.Checked = true;
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            VolverAlMenuPrincipal();
        }

        private void VolverAlMenuPrincipal()
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void comboBox_tiposDePublicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tipoSeleccionado = comboBox_TiposDePublicacion.Text;
            if (tipoSeleccionado == "Compra inmediata")
            {
                label_stock.Text = "Stock";
                label_precio.Text = "Precio por unidad";
            }
            else
            {
                label_stock.Text = "Cantidad";
                label_precio.Text = "Precio inicial";
            }
        }
    }
}