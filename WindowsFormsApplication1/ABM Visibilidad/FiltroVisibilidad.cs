using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Visibilidad
{
    public partial class FiltroVisibilidad : Form
    {
        private DBMapper mapper = new DBMapper();

        public FiltroVisibilidad()
        {
            InitializeComponent();
        }

        private void FiltroVisibilidad_Load(object sender, EventArgs e)
        {
            CargarVisibilidad();
            OcultarColumnasQueNoDebenVerse();
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridView_Visibilidad.Columns["visib_id"].Visible = false;
        }

        private void CargarVisibilidad()
        {
            dataGridView_Visibilidad.DataSource = mapper.SelectVisibilidadesParaFiltro();
            CargarColumnaModificacion();
            CargarColumnaEliminar();
        }

        private void CargarColumnaModificacion()
        {
            if (dataGridView_Visibilidad.Columns.Contains("Modificar"))
                dataGridView_Visibilidad.Columns.Remove("Modificar");
            DataGridViewButtonColumn botonColumnaModificar = new DataGridViewButtonColumn();
            botonColumnaModificar.Text = "Modificar";
            botonColumnaModificar.Name = "Modificar";
            botonColumnaModificar.UseColumnTextForButtonValue = true;
            dataGridView_Visibilidad.Columns.Add(botonColumnaModificar);
        }

        private void CargarColumnaEliminar()
        {
            if (dataGridView_Visibilidad.Columns.Contains("Eliminar"))
                dataGridView_Visibilidad.Columns.Remove("Eliminar");
            DataGridViewButtonColumn botonColumnaEliminar = new DataGridViewButtonColumn();
            botonColumnaEliminar.Text = "Eliminar";
            botonColumnaEliminar.Name = "Eliminar";
            botonColumnaEliminar.UseColumnTextForButtonValue = true;
            dataGridView_Visibilidad.Columns.Add(botonColumnaEliminar);
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            String filtro = CalcularFiltro();
            dataGridView_Visibilidad.DataSource = mapper.SelectVisibilidadesParaFiltroConFiltro(filtro);
        }

        private String CalcularFiltro()
        {
            String filtro = "";
            if (textBox_Descripcion.Text != "") filtro += " AND visib_desc like '" + textBox_Descripcion.Text + "%'";
            return filtro;
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Descripcion.Text = "";
            CargarVisibilidad();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void dataGridView_Visibilidad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Controla que la celda que se clickeo fue la de modificar
            if (e.ColumnIndex == dataGridView_Visibilidad.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                String idVisibilidadAModificiar = dataGridView_Visibilidad.Rows[e.RowIndex].Cells["visib_id"].Value.ToString();
                new EditarVisibilidad(idVisibilidadAModificiar).ShowDialog();
                CargarVisibilidad();
                return;
            }
            if (e.ColumnIndex == dataGridView_Visibilidad.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                String idVisibilidadAEliminar = dataGridView_Visibilidad.Rows[e.RowIndex].Cells["visib_id"].Value.ToString();
                Boolean resultado = mapper.EliminarVisibilidad(Convert.ToDecimal(idVisibilidadAEliminar), "Visibilidad");
                if (resultado) MessageBox.Show("Se elimino correctamente");
                CargarVisibilidad();
                return;
            }
        }
    }
}