using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MercadoEnvio.Calificar_Vendedor
{
    public partial class Listado : Form
    {
        private DBMapper comunicar = new DBMapper();

        public Listado()
        {
            InitializeComponent();
        }

        private void Calificar_Load(object sender, EventArgs e)
        {
            CargarCompras();
            OcultarColumnasQueNoDebenVerse();
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridViewCompras.Columns["id"].Visible = false;
        }

        private void CargarCompras()
        {
            dataGridViewCompras.DataSource = comunicar.SelectDataTableConUsuario("c.comp_id id, u.usr_usuario Vendedor, p.publi_descripcion Publicacion, c.comp_cantidad Cantidad, p.publi_tipo Tipo, convert(varchar, c.comp_fecha, 102) Fecha", "NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p, NET_A_CERO.Usuarios u", "isnull(c.comp_calif_id,0) = 0 AND c.comp_publi_id = p.publi_id and p.publi_usr_id = u.usr_id AND c.comp_usr_id = @idUsuario");
            CargarColumnaCalificacion();
        }

        private void CargarColumnaCalificacion()
        {
            if (dataGridViewCompras.Columns.Contains("Calificar"))
                dataGridViewCompras.Columns.Remove("Calificar");
            DataGridViewButtonColumn botonColumnaModificar = new DataGridViewButtonColumn();
            botonColumnaModificar.Text = "Calificar";
            botonColumnaModificar.Name = "Calificar";
            botonColumnaModificar.UseColumnTextForButtonValue = true;
            dataGridViewCompras.Columns.Add(botonColumnaModificar);
            dataGridViewCompras.CellClick +=
                new DataGridViewCellEventHandler(dataGridViewCompras_CellClick);
        }

        private void dataGridViewCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Controla que la celda que se clickeo calificar vendedor
            if (e.ColumnIndex == dataGridViewCompras.Columns["Calificar"].Index && e.RowIndex >= 0)
            {
                Decimal idCompraParaCalificar = Convert.ToDecimal(dataGridViewCompras.Rows[e.RowIndex].Cells[0].Value);
                new Calificar(idCompraParaCalificar).ShowDialog();
                CargarCompras();
            }
            return;
        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

    }
}