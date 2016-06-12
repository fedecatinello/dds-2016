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

namespace MercadoEnvio.Editar_Publicacion
{
    public partial class FiltrarPublicacion : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        public Object SelectedItem { get; set; }
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        private DBMapper mapper = new DBMapper();
   
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
            comboBoxRubro1.DataSource = mapper.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBoxRubro1.ValueMember = "rubro_desc_larga";
            comboBoxRubro1.SelectedIndex = -1;

            comboBoxRubro2.DataSource = mapper.SelectDataTable("rubro_desc_larga", "NET_A_CERO.Rubros");
            comboBoxRubro2.ValueMember = "rubro_desc_larga";
            comboBoxRubro2.SelectedIndex = -1;
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridView_Publicacion.Columns["publi_id"].Visible = false;
        }

        private void CargarPublicacion()
        {
            dataGridView_Publicacion.DataSource = mapper.SelectPublicacionesParaFiltro();
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
            dataGridView_Publicacion.DataSource = FiltrarPublicaciones(filtro);
        }

        private String CalcularFiltro()
        {
            parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@idUsuario", idUsuarioActual));

            String filtro = "";

            if (textBoxDescripcion.Text != "")
            {
                filtro += " AND p.publi_descripcion like '%" + textBoxDescripcion.Text + "%'";
            }

            if (comboBoxRubro1.Text != "")
            {
                String idRubro1 = Convert.ToString(mapper.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", comboBoxRubro1.Text));
                parametros.Add(new SqlParameter("@idRubro1", idRubro1));
                if (comboBoxRubro2.Text != "")
                {
                    filtro += " AND ( rxp.rubro_id = @idRubro1 ";
                }
                else
                {
                    filtro += " AND rxp.rubro_id = @idRubro1 ";
                }
            }

            if (comboBoxRubro2.Text != "")
            {
                String idRubro2 = Convert.ToString(mapper.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", comboBoxRubro2.Text));
                parametros.Add(new SqlParameter("@idRubro2", idRubro2));
                if (comboBoxRubro1.Text != "")
                {
                    filtro += " OR rxp.rubro_id = @idRubro2 ) ";
                }
                else
                {
                    filtro += " AND rxp.rubro_id = @idRubro2 ";
                }

            }
            return filtro;
        }

        public DataTable FiltrarPublicaciones(String filtro)
        {
           SqlCommand command = new SqlCommand();
           command = QueryBuilder.Instance.build("SELECT " + 
               "p.publi_id, u.usr_usuario Usuario, p.publi_descripcion Descripcion, p.publi_fec_inicio 'Fecha de inicio', p.publi_fec_vencimiento 'Fecha de vencimiento', r.rubro_desc_larga Rubro, v.visib_desc Visibilidad, p.publi_preguntas 'Permite preguntas', p.publi_stock Stock, p.publi_precio Precio" 
               + " FROM " 
               + "NET_A_CERO.Publicaciones p, NET_A_CERO.Rubros r, NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u, NET_A_CERO.Rubro_x_Publicacion rxp" 
               + " WHERE " 
               + "rxp.rubro_id = r.rubro_id AND rxp.publi_id = p.publi_id AND p.publi_visib_id = v.visib_id AND p.publi_usr_id = u.usr_id " + filtro, parametros);
            command.CommandTimeout = 0;  
            DataSet datos = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(datos);
            return datos.Tables[0];
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            CargarPublicacion();
            OcultarColumnasQueNoDebenVerse();
            CargarRubros();
            textBoxDescripcion.Text = "";
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