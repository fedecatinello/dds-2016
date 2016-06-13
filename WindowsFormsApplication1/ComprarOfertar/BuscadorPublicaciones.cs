using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.Comprar_Ofertar
{
    public partial class BuscadorPublicaciones : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        public Object SelectedItem { get; set; }
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        private DBMapper mapper = new DBMapper();

        DataTable tablaTemporal;
        int totalPaginas;
        int totalPublicaciones;
        int publicacionesPorPagina = 10;
        int paginaActual;
        int ini;
        int fin;

        public BuscadorPublicaciones()
        {
            InitializeComponent();
        }
                
        private void BuscardorPublicaciones_Load(object sender, EventArgs e)
        {            
            CargarRubros();
            OcultarColumnasQueNoDebenVerse();
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridViewBuscadorPubli.Columns["visib_precio"].Visible = false;
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

        private void botonBuscar_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", idUsuarioActual));
            DataTable busquedaTemporal = new DataTable();
            String filtro = " AND publicaciones.publi_usr_id != @usuario";
           
            if (textBoxDescripcion.Text != "")
            {
                filtro += " AND publicaciones.publi_descripcion LIKE '%" + textBoxDescripcion.Text + "%'";                
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
          

            String query = "SELECT DISTINCT(publicaciones.publi_id) ,visibilidad.visib_precio, publicaciones.publi_descripcion Descripcion , " +
                    "(CASE WHEN (publicaciones.publi_tipo = 'Subasta' AND (SELECT COUNT(*) FROM NET_A_CERO.VistaOfertaMaxima vista WHERE vista.vista_publi_id = publicaciones.publi_id) = 1)" +
                        "THEN (SELECT vista.vista_precioMax FROM NET_A_CERO.VistaOfertaMaxima vista WHERE vista.vista_publi_id = publicaciones.publi_id) " +
                            "ELSE publicaciones.publi_precio END) Precio, " +
                        "publicaciones.publi_tipo 'Tipo Publicacion' " +
                    "FROM NET_A_CERO.Publicaciones publicaciones, NET_A_CERO.Visibilidad visibilidad, NET_A_CERO.Rubro_x_Publicacion rxp " +
                    "WHERE publicaciones.publi_visib_id = visibilidad.visib_id AND publicaciones.publi_estado_id = (SELECT estado_id FROM NET_A_CERO.Estado WHERE estado_desc='Activa') " +
                            " and publicaciones.publi_id = rxp.publi_id "
                                      + filtro + " ORDER BY visibilidad.visib_precio DESC";
            
            
            command = QueryBuilder.Instance.build(query, parametros);
            adapter.SelectCommand = command;            
            adapter.Fill(busquedaTemporal);

            int cantFilas = busquedaTemporal.Rows.Count;
            if (cantFilas == 0)
            {
                MessageBox.Show("No hay resultados");
                return;
            }
            else
            {
                tablaTemporal = busquedaTemporal;
                calcularPaginas();
                ini = 0;
                if (totalPublicaciones > 9)
                {
                    fin = 9;
                }
                else
                {
                    fin = totalPublicaciones;
                }
                calcularPaginas();
                dataGridViewBuscadorPubli.DataSource = paginarDataGridView(ini, fin);
                dataGridViewBuscadorPubli.Columns[0].Visible = false;
                mostrarNrosPaginas(ini);
            }
            OcultarColumnasQueNoDebenVerse();
            AgregarBotonVerPublicacion();
        }

        private void calcularPaginas()
        {
            totalPublicaciones = tablaTemporal.Rows.Count - 1;
            totalPaginas = totalPublicaciones / publicacionesPorPagina;
            if ((totalPublicaciones / publicacionesPorPagina) > 0)
            {
                totalPaginas += 1;
            }
        }

        private DataTable paginarDataGridView(int ini, int fin)
        {
            DataTable publicacionesDeUnaPagina = new DataTable();
            publicacionesDeUnaPagina = tablaTemporal.Clone();
            for (int i = ini; i <= fin; i++)
            {
                publicacionesDeUnaPagina.ImportRow(tablaTemporal.Rows[i]);
            }
            return publicacionesDeUnaPagina;
        }

        private void mostrarNrosPaginas(int ini)
        {
            paginaActual = (ini / publicacionesPorPagina) + 1;
            labelNrosPagina.Text = "Pagina " + paginaActual + "/" + totalPaginas;
        }        

        private bool VerificarSiSeBusco()
        {
            if (totalPaginas == 0)
            {
                MessageBox.Show("Aun no buscaste nada");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool sePuedeRetrocederPaginas()
        {
            if (VerificarSiSeBusco() == false)
            {
                return false;
            }
            else
            {
                if (paginaActual == 1)
                {
                    MessageBox.Show("Ya estas en la 1º pagina");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void botonPrimeraPagina_Click(object sender, EventArgs e)
        {
            if (sePuedeRetrocederPaginas())
            {
                ini = 0;
                fin = 9;
                dataGridViewBuscadorPubli.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void botonPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (sePuedeRetrocederPaginas())
            {
                ini -= publicacionesPorPagina;
                if (fin != totalPublicaciones)
                {
                    fin -= publicacionesPorPagina;
                }
                else
                {
                    fin = ini + 9;
                }

                dataGridViewBuscadorPubli.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private bool sePuedeAvanzarPaginas()
        {
            if (VerificarSiSeBusco() == false)
            {
                return false;
            }
            else
            {
                if (paginaActual == totalPaginas)
                {
                    MessageBox.Show("Ya estas en la ultima pagina");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void botonPaginaSiguiente_Click(object sender, EventArgs e)
        {
            if (sePuedeAvanzarPaginas())
            {
                ini += publicacionesPorPagina;
                if ((fin + publicacionesPorPagina) < totalPublicaciones)          
                
                {
                    fin += publicacionesPorPagina;                    
                }
                else
                {
                    fin = totalPublicaciones;
                }                
                dataGridViewBuscadorPubli.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }


        private void botonUltimaPagina_Click(object sender, EventArgs e)
        {
            if (sePuedeAvanzarPaginas())
            {
                ini = (totalPaginas - 1) * publicacionesPorPagina;
                fin = totalPublicaciones;
                dataGridViewBuscadorPubli.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void AgregarBotonVerPublicacion()
        {
            if (dataGridViewBuscadorPubli.Columns.Contains("Ver Publicacion"))
                dataGridViewBuscadorPubli.Columns.Remove("Ver Publicacion");
            DataGridViewButtonColumn buttons = new DataGridViewButtonColumn();
            {
                buttons.HeaderText = "Ver Publicacion";
                buttons.Text = "Ver Publicacion";
                buttons.Name = "Ver Publicacion";
                buttons.UseColumnTextForButtonValue = true;
                buttons.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                buttons.FlatStyle = FlatStyle.Standard;
                buttons.CellTemplate.Style.BackColor = Color.Honeydew;
                dataGridViewBuscadorPubli.CellClick +=
                    new DataGridViewCellEventHandler(dataGridView1_CellClick);
            }

            dataGridViewBuscadorPubli.Columns.Add(buttons);


        }        

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewBuscadorPubli.Columns["Ver Publicacion"].Index)
            {
                int idPublicacionElegida = Convert.ToInt32(dataGridViewBuscadorPubli.Rows[e.RowIndex].Cells["publi_id"].Value);
                this.Hide();
                new VerPublicacion(idPublicacionElegida).ShowDialog();
                this.Close();
            }
        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxDescripcion.Clear();

            //Limpio grilla siempre y cuando haya arrojado resultados
            if (dataGridViewBuscadorPubli.DataSource != null)
            {
                OcultarColumnasQueNoDebenVerse();
                if (dataGridViewBuscadorPubli.Columns.Contains("Ver Publicacion"))
                    dataGridViewBuscadorPubli.Columns.Remove("Ver Publicacion");
                dataGridViewBuscadorPubli.DataSource = null;
            }

            comboBoxRubro1.SelectedIndex = -1;
            comboBoxRubro2.SelectedIndex = -1;
            labelNrosPagina.Text = "";
            
        }

        private void botonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }
                
    }
}