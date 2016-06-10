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
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.ABM_Cliente
{
    public partial class FiltroCliente : Form
    {
        private DBMapper mapper = new DBMapper();

        public FiltroCliente()
        {
            InitializeComponent();
        }

        private void FiltroCliente_Load(object sender, EventArgs e)
        {
            CargarTiposDeDocumento();
            CargarClientes();
            OcultarColumnasQueNoDebenVerse();
        }

        private void CargarTiposDeDocumento()//Chequear como hacer para que se sincronice con el de agregarCliente
        {          
            comboBox_TipoDeDoc.Items.Add("DNI - Documento Nacional de Identidad");
            comboBox_TipoDeDoc.Items.Add("Pasaporte");
            comboBox_TipoDeDoc.Items.Add("LC - Libreta Civica");
            comboBox_TipoDeDoc.Items.Add("LE - Libreta de Enrolamiento");       
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridView_Cliente.Columns["cli_id"].Visible = false;
        }

        private void CargarClientes()
        {
            dataGridView_Cliente.DataSource = mapper.SelectClientesParaFiltro();
            CargarColumnaModificacion();
            CargarColumnaEliminar();
        }

        private void CargarColumnaModificacion()
        {
            if (dataGridView_Cliente.Columns.Contains("Modificar"))
                dataGridView_Cliente.Columns.Remove("Modificar");
            DataGridViewButtonColumn botonColumnaModificar = new DataGridViewButtonColumn();
            botonColumnaModificar.Text = "Modificar";
            botonColumnaModificar.Name = "Modificar";
            botonColumnaModificar.UseColumnTextForButtonValue = true;
            dataGridView_Cliente.Columns.Add(botonColumnaModificar);
        }

        private void CargarColumnaEliminar()
        {
            if (dataGridView_Cliente.Columns.Contains("Eliminar"))
                dataGridView_Cliente.Columns.Remove("Eliminar");
            DataGridViewButtonColumn botonColumnaEliminar = new DataGridViewButtonColumn();
            botonColumnaEliminar.Text = "Eliminar";
            botonColumnaEliminar.Name = "Eliminar";
            botonColumnaEliminar.UseColumnTextForButtonValue = true;
            dataGridView_Cliente.Columns.Add(botonColumnaEliminar);
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            String filtro = CalcularFiltro();
            dataGridView_Cliente.DataSource = mapper.SelectClientesParaFiltroConFiltro(filtro);
        }

        private String CalcularFiltro()
        {
            String filtro = "";
            if (textBox_Nombre.Text != "") filtro += "AND " + "cli.cli_nombre LIKE '" + textBox_Nombre.Text + "%'";
            if (textBox_Apellido.Text != "") filtro += "AND " + "cli.cli_apellido LIKE '" + textBox_Apellido.Text + "%'";
            if (textBox_Mail.Text != "") filtro += "AND " + "cont.cont_mail LIKE '" + textBox_Mail.Text + "%'";
            if (textBox_NumeroDeDoc.Text != "") filtro += "AND " + "cli.cli_dni LIKE '" + textBox_NumeroDeDoc.Text + "%'";
            if (comboBox_TipoDeDoc.Text != "") filtro += "AND " + "cli.cli_tipo_dni LIKE '" + comboBox_TipoDeDoc.Text + "%'";
            return filtro;
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Nombre.Text = "";
            textBox_Apellido.Text = "";
            textBox_Mail.Text = "";
            textBox_NumeroDeDoc.Text = "";
            comboBox_TipoDeDoc.SelectedIndex = -1;
            CargarClientes();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void dataGridView_Cliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Controla que la celda que se clickeo fue la de modificar
            if (e.ColumnIndex == dataGridView_Cliente.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                String idClienteAModificar = dataGridView_Cliente.Rows[e.RowIndex].Cells["cli_id"].Value.ToString();
                new EditarCliente(idClienteAModificar).ShowDialog();
                CargarClientes();
                return;
            }
            if (e.ColumnIndex == dataGridView_Cliente.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                String idClienteAEliminar = dataGridView_Cliente.Rows[e.RowIndex].Cells["cli_id"].Value.ToString();
                Boolean resultado = mapper.EliminarCliente(Convert.ToInt32(idClienteAEliminar), "Clientes");
                if (resultado) MessageBox.Show("Se elimino correctamente");
                CargarClientes();
                return;
            }
        }
    }
}