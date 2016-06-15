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
        private IList<SqlParameter> parametros;
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
           labelNrosPagina.Text = "";
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridViewFacturas.Columns["fact_id"].Visible = false;
        }


        
        private void botonBuscar_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", idUsuarioActual));
            DataTable busquedaTemporal = new DataTable();
            String filtro = "";
            
            if (textBox_ImporteDesde.Text != "")
            {

                if (textBox_ImporteHasta.Text != "") //Ambos importes fueron completados
                {
                    if (Convert.ToDecimal(textBox_ImporteHasta.Text) < Convert.ToDecimal(textBox_ImporteDesde.Text))
                    {
                        MessageBox.Show("El importe desde debe ser menor que el importe hasta");
                        return;
                    }
                    else
                        filtro += " AND f.fact_monto BETWEEN " + textBox_ImporteDesde.Text + " AND " + textBox_ImporteHasta.Text;
                }
                else //Solo importe desde fue completado
                    filtro += " AND f.fact_monto > " + textBox_ImporteDesde.Text;
            }
            
            if(textBox_ImporteHasta.Text != "") //Solo importe hasta fue completado
            {
                filtro += " AND f.fact_monto < " + textBox_ImporteHasta.Text;
            }

            if (textBox_FechaDesde.Text != "")
            {

                if (textBox_FechaHasta.Text != "") //Ambas fechas fueron completadas
                {
                    if (DateTime.Compare(Convert.ToDateTime(textBox_FechaDesde.Text), Convert.ToDateTime(textBox_FechaHasta.Text)) > 0)
                    {
                        MessageBox.Show("La fecha desde debe ser anterior que la fecha hasta");
                        return;
                    }
                    else
                      filtro += " AND f.fact_fecha BETWEEN '" + textBox_FechaDesde.Text + "' AND '" + textBox_FechaHasta.Text + "'";
                }
                else //Solo fecha desde fue completada
                    filtro += " AND f.fact_fecha > '" + textBox_FechaDesde.Text + "'";
            }

            if (textBox_FechaHasta.Text != "") //Solo fecha hasta fue completada
            {
                filtro += " AND f.fact_fecha < '" + textBox_FechaHasta.Text + "'";
            }

            /** Query de las facturas del vendedor **/

            String query = "SELECT DISTINCT(f.fact_id), f.fact_fecha 'Fecha', f.fact_monto 'Monto', f.fact_destinatario 'Destinatario', f.fact_forma_pago 'Forma de Pago', p.publi_descripcion 'Publicacion' " +
                            "FROM NET_A_CERO.Facturas f, NET_A_CERO.Publicaciones p " +
                            "WHERE f.fact_destinatario = @usuario AND f.fact_publi_id = p.publi_id AND p.publi_usr_id = @usuario " + filtro;
            
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
                int idFacturaElegida = Convert.ToInt32(dataGridViewFacturas.Rows[e.RowIndex].Cells["fact_id"].Value);
                this.Hide();
                new VerFactura(idFacturaElegida).ShowDialog();
                this.Close();
            }
        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            //Limpio filtros de busqueda
            textBox_FechaDesde.Clear();
            textBox_FechaHasta.Clear();
            textBox_ImporteDesde.Clear();
            textBox_ImporteHasta.Clear();

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

        private void button_FechaHasta_Click(object sender, EventArgs e)
        {
            monthCalendar_FechaHasta.Visible = true;
        }

        private void button_FechaDesde_Click(object sender, EventArgs e)
        {
            monthCalendar_FechaDesde.Visible = true;
        }

        private void monthCalendar_FechaDesde_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaDesde.Text = e.Start.ToShortDateString();
            monthCalendar_FechaDesde.Visible = false;
        }

        private void monthCalendar_FechaHasta_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaHasta.Text = e.Start.ToShortDateString();
            monthCalendar_FechaHasta.Visible = false;
        }


    }
}
