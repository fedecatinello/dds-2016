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

namespace MercadoEnvio.ABM_Visibilidad
{
    public partial class EditarVisibilidad : Form
    {
        private Decimal idVisibilidad;
        private DBMapper mapper = new DBMapper();

        public EditarVisibilidad(String idVisibilidad)
        {
            InitializeComponent();
            this.idVisibilidad = Convert.ToDecimal(idVisibilidad);
        }

        private void EditarVisibilidad_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            Visibilidad visibilidad = mapper.ObtenerVisibilidad(idVisibilidad);
            textBox_Descripcion.Text = visibilidad.GetDescripcion();
            textBox_PrecioPorPublicar.Text = visibilidad.GetPrecioPorPublicar();
            textBox_PorcentajePorVenta.Text = visibilidad.GetPorcentajePorVenta();
            comboBox_Grado.Text = visibilidad.GetGrado();
            checkBox_Envios.Checked = Convert.ToBoolean(mapper.SelectFromWhere("visib_envios", "Visibilidad", "visib_id", idVisibilidad));
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            String descripcion = textBox_Descripcion.Text;
            String precioPorPublicar = textBox_PrecioPorPublicar.Text;
            String porcentajePorVenta = textBox_PorcentajePorVenta.Text;
            String grado = comboBox_Grado.Text;
            Boolean envios = checkBox_Envios.Checked;

            // Update Visibilidad
            try
            {
                Visibilidad visibilidad = new Visibilidad();
                visibilidad.SetDescripcion(descripcion);
                visibilidad.SetPrecioPorPublicar(precioPorPublicar);
                visibilidad.SetPorcentajePorVenta(porcentajePorVenta);
                visibilidad.SetGrado(grado);
                visibilidad.SetEnvios(envios);
                Boolean pudoModificar = mapper.Modificar(idVisibilidad, visibilidad);
                if (pudoModificar) MessageBox.Show("La visibilidad se modifico correctamente");
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
            catch (VisibilidadYaExisteException exception)
            {
                MessageBox.Show("Ya existe esa visibilidad");
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