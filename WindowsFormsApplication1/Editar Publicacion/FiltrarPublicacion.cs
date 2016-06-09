using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MercadoEnvio.Editar_Publicacion
{
    public partial class FiltrarPublicacion : Form
    {
        private DBMapper comunicador = new DBMapper();
        
        public FiltrarPublicacion()
        {
            InitializeComponent();
        }

        private void FiltrarPublicacion_Load(object sender, EventArgs e)
        {
            CargarPublicacion();
            OcultarColumnasQueNoDebenVerse();
            CargarRubros(); 
        }

        private void CargarRubros()
        {
            comboBoxRubro.DataSource = comunicador.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBoxRubro.ValueMember = "rubro_desc_larga";
            comboBoxRubro.SelectedIndex = -1;
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridView_Publicacion.Columns["publi_id"].Visible = false;
        }

        private void CargarPublicacion()
        {
            dataGridView_Publicacion.DataSource = comunicador.SelectPublicacionesParaFiltro();
            CargarColumnaModificacion();
        }

        private void CargarColumnaModificacion()
        {
            if (dataGridView_Publicacion.Columns.Contains("Modificar"))
                dataGridView_Publicacion.Columns.Remove("Modificar");
            DataGridViewButtonColumn botonColumnaModificar = new DataGridViewButtonColumn();
            botonColumnaModificar.Text = "Modificar";
            botonColumnaModificar.Name = "Modificar";
            botonColumnaModificar.UseColumnTextForButtonValue = true;
            dataGridView_Publicacion.Columns.Add(botonColumnaModificar);
            dataGridView_Publicacion.CellClick +=
                new DataGridViewCellEventHandler(dataGridView_Publicacion_CellClick);
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            String filtro = CalcularFiltro();
            dataGridView_Publicacion.DataSource = comunicador.SelectPublicacionesParaFiltroConFiltro(filtro);
        }

        private String CalcularFiltro()
        {
            String filtro = "";
            return filtro;
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            CargarPublicacion();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {

            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void dataGridView_Publicacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Controla que la celda que se clickeo fue la de modificar
            if (e.ColumnIndex == dataGridView_Publicacion.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                String idPublicacionAModificiar = dataGridView_Publicacion.Rows[e.RowIndex].Cells["publi_id"].Value.ToString();
                new Editar_Publicacion.EditarPublicacion(idPublicacionAModificiar).ShowDialog();
                CargarPublicacion();
                return;
            }
        }
    }
}