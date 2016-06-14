using MercadoEnvio.DataProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercadoEnvio.Consulta_Facturas_Vendedor
{
    public partial class ListadoFacturas : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        public Object SelectedItem { get; set; }
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        private DBMapper mapper = new DBMapper();

        DataTable tablaTemporal;
        int totalPaginas;
        int totalFacturas;
        int facturasPorPagina = 10;
        int paginaActual;
        int ini;
        int fin;

        public ListadoFacturas()
        {
            InitializeComponent();
        }

        private void ListadoFacturas_Load(object sender, EventArgs e)
        {
            OcultarColumnasQueNoDebenVerse();
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridViewFacturas.Columns["fact_id"].Visible = false;
            dataGridViewFacturas.Columns["fact_publi_id"].Visible = false;
        }

        private void botonBuscar_Click(object sender, EventArgs e) /** Faltan codear los filtros **/
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", idUsuarioActual));
            DataTable busquedaTemporal = new DataTable();
            /*String filtro = " AND publicaciones.publi_usr_id != @usuario";
           
            if (textBox_Contenido.Text != "")
            {
                filtro += " AND publicaciones.publi_descripcion LIKE '%" + textBox_Contenido.Text + "%'";                
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

            } */
          

           /* String query = "SELECT DISTINCT(publicaciones.publi_id) ,visibilidad.visib_precio, publicaciones.publi_descripcion Descripcion , " +
                    "(CASE WHEN (publicaciones.publi_tipo = 'Subasta' AND (SELECT COUNT(*) FROM NET_A_CERO.VistaOfertaMaxima vista WHERE vista.vista_publi_id = publicaciones.publi_id) = 1)" +
                        "THEN (SELECT vista.vista_precioMax FROM NET_A_CERO.VistaOfertaMaxima vista WHERE vista.vista_publi_id = publicaciones.publi_id) " +
                            "ELSE publicaciones.publi_precio END) Precio, " +
                        "publicaciones.publi_tipo 'Tipo Publicacion' " +
                    "FROM NET_A_CERO.Publicaciones publicaciones, NET_A_CERO.Visibilidad visibilidad, NET_A_CERO.Rubro_x_Publicacion rxp " +
                    "WHERE publicaciones.publi_visib_id = visibilidad.visib_id AND publicaciones.publi_estado_id = (SELECT estado_id FROM NET_A_CERO.Estado WHERE estado_desc='Activa') " +
                            " and publicaciones.publi_id = rxp.publi_id "
                                      + filtro + " ORDER BY visibilidad.visib_precio DESC"; */
            
            String query = "";
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
                if (totalFacturas > 9)
                {
                    fin = 9;
                }
                else
                {
                    fin = totalFacturas;
                }
                calcularPaginas();
                dataGridViewFacturas.DataSource = paginarDataGridView(ini, fin);
                dataGridViewFacturas.Columns[0].Visible = false;
                mostrarNrosPaginas(ini);
            }
            OcultarColumnasQueNoDebenVerse();
            AgregarBotonVerFactura();
        }

        private void calcularPaginas()
        {
            totalFacturas = tablaTemporal.Rows.Count - 1;
            totalPaginas = totalFacturas / facturasPorPagina;
            if ((totalFacturas / facturasPorPagina) > 0)
            {
                totalPaginas += 1;
            }
        }

        private DataTable paginarDataGridView(int ini, int fin)
        {
            DataTable facturasDeUnaPagina = new DataTable();
            facturasDeUnaPagina = tablaTemporal.Clone();
            for (int i = ini; i <= fin; i++)
            {
                facturasDeUnaPagina.ImportRow(tablaTemporal.Rows[i]);
            }
            return facturasDeUnaPagina;
        }

        private void mostrarNrosPaginas(int ini)
        {
            paginaActual = (ini / facturasPorPagina) + 1;
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
                dataGridViewFacturas.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void botonPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (sePuedeRetrocederPaginas())
            {
                ini -= facturasPorPagina;
                if (fin != totalFacturas)
                {
                    fin -= facturasPorPagina;
                }
                else
                {
                    fin = ini + 9;
                }

                dataGridViewFacturas.DataSource = paginarDataGridView(ini, fin);
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
                ini += facturasPorPagina;
                if ((fin + facturasPorPagina) < totalFacturas)
                {
                    fin += facturasPorPagina;
                }
                else
                {
                    fin = totalFacturas;
                }
                dataGridViewFacturas.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void botonUltimaPagina_Click(object sender, EventArgs e)
        {
            if (sePuedeAvanzarPaginas())
            {
                ini = (totalPaginas - 1) * facturasPorPagina;
                fin = totalFacturas;
                dataGridViewFacturas.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void AgregarBotonVerFactura()
        {
            if (dataGridViewFacturas.Columns.Contains("Ver Factura"))
                dataGridViewFacturas.Columns.Remove("Ver Factura");
            DataGridViewButtonColumn buttons = new DataGridViewButtonColumn();
            {
                buttons.HeaderText = "Ver Factura";
                buttons.Text = "Ver Factura";
                buttons.Name = "Ver Factura";
                buttons.UseColumnTextForButtonValue = true;
                buttons.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.AllCells;
                buttons.FlatStyle = FlatStyle.Standard;
                buttons.CellTemplate.Style.BackColor = Color.Honeydew;
                dataGridViewFacturas.CellClick +=
                    new DataGridViewCellEventHandler(dataGridView1_CellClick);
            }

            dataGridViewFacturas.Columns.Add(buttons);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewFacturas.Columns["Ver Factura"].Index)
            {
                int idFacturaElegida = Convert.ToInt32(dataGridViewFacturas.Rows[e.RowIndex].Cells["publi_id"].Value);
                this.Hide();
                new VerFactura(idFacturaElegida).ShowDialog();
                this.Close();
            }
        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            //Limpio filtros de busqueda
            textBox_Contenido.Clear();
            textBox_Contenido.Clear();
            textBox_Fecha.Clear();
            textBox_Importe.Clear();

            //Limpio grilla siempre y cuando haya arrojado resultados
            if (dataGridViewFacturas.DataSource != null)
            {
                OcultarColumnasQueNoDebenVerse();
                if (dataGridViewFacturas.Columns.Contains("Ver Factura"))
                    dataGridViewFacturas.Columns.Remove("Ver Factura");
                dataGridViewFacturas.DataSource = null;
            }

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
