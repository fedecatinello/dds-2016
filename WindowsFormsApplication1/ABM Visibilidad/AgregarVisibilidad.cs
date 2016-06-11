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
    public partial class AgregarVisibilidad : Form
    {
        private DBMapper mapper = new DBMapper();

        public AgregarVisibilidad()
        {
            InitializeComponent();
        }

        private void AgregarVisibilidad_Load(object sender, EventArgs e)
        {
            CargarGradoVisibilidad();
        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            String descripcion = textBox_Descripcion.Text;
            String precioPorPublicar = textBox_PrecioPorPublicar.Text;
            String porcentajePorVenta = textBox_PorcentajePorVenta.Text;
            String grado = comboBox_Grado.Text;
            //String duracion = textBox_Duracion.Text;

            // Inserto la Visibilidad en la DB
            try
            {
                Visibilidad visibilidad = new Visibilidad();
                visibilidad.SetDescripcion(descripcion);
                visibilidad.SetPrecioPorPublicar(precioPorPublicar);
                visibilidad.SetPorcentajePorVenta(porcentajePorVenta);
                visibilidad.SetGrado(grado);
                //visibilidad.SetDuracion(duracion);
                bool isPersisted = mapper.CrearVisibilidad(visibilidad);
                if (isPersisted) MessageBox.Show("La visibilidad fue creada");
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
            catch (VisibilidadYaExisteException exception)
            {
                MessageBox.Show("La visibilidad ya existe");
                return;
            }

            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Descripcion.Text = "";
            textBox_PorcentajePorVenta.Text = "";
            textBox_PrecioPorPublicar.Text = "";
            comboBox_Grado.SelectedIndex = -1;
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        public void CargarGradoVisibilidad()
        {
            comboBox_Grado.Items.Add("Comisión por tipo de publicación");
            comboBox_Grado.Items.Add("Comisión por producto vendido");
            comboBox_Grado.Items.Add("Comisión por envío del producto");
            comboBox_Grado.Items.Add("Comisión gratuita");
        }
    }
}