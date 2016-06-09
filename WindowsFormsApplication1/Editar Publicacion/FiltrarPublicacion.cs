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
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        public Object SelectedItem { get; set; }
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
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
            comboBoxRubro1.DataSource = comunicador.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBoxRubro1.ValueMember = "rubro_desc_larga";
            comboBoxRubro1.SelectedIndex = -1;

            comboBoxRubro2.DataSource = comunicador.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBoxRubro2.ValueMember = "rubro_desc_larga";
            comboBoxRubro2.SelectedIndex = -1;
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
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", 90/*idUsuarioActual*/));
            DataTable busquedaTemporal = new DataTable();
            String filtro = "and publicaciones.publi_usr_id != @usuario";

            if (textBoxDescripcion.Text != "")
            {
                filtro += " and publicaciones.publi_descripcion like '%" + textBoxDescripcion.Text + "%'";
            }

            if (comboBoxRubro1.Text != "")
            {
                String idRubro1 = Convert.ToString(comunicador.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", comboBoxRubro1.Text));
                parametros.Add(new SqlParameter("@idRubro1", idRubro1));
                filtro += " and ( rxp.rubro_id = @idRubro1 ";
            }

            if (comboBoxRubro2.Text != "")
            {
                String idRubro2 = Convert.ToString(comunicador.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", comboBoxRubro2.Text));
                parametros.Add(new SqlParameter("@idRubro2", idRubro2));
                if (comboBoxRubro1.Text != "")
                {
                    filtro += " or rxp.rubro_id = @idRubro2 ) ";
                }
                else
                {
                    filtro += " and rxp.rubro_id = @idRubro2 ";
                }

            }
            else
            {
                filtro += ") ";
            }

            MessageBox.Show("Falta completar campo: " + filtro);
            //String filtro = CalcularFiltro();
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
            OcultarColumnasQueNoDebenVerse();
            CargarRubros(); 
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