using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MercadoEnvio.Historial_Cliente
{
    public partial class Historial : Form
    {
        private String query;
        private SqlCommand command;
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private DBMapper mapper = new DBMapper();

        /** Paginacion variables **/
        DataTable tablaTemporal;
        int totalPaginas;
        int totalPublicaciones;
        int publicacionesPorPagina = 10;
        int paginaActual;
        int ini;
        int fin;

        public Historial()
        {
            InitializeComponent();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            CargarOpciones();
            CargarDatos();
        }

        private void CargarOpciones()
        {
            DataTable opciones = new DataTable();
            opciones.Columns.Add("opciones");
            opciones.Rows.Add("Compras");
            opciones.Rows.Add("Ofertas");
            opciones.Rows.Add("Calificaciones");
            comboBox_opciones.DataSource = opciones;
            comboBox_opciones.ValueMember = "opciones";
            comboBox_opciones.SelectedIndex = -1;
        }

        private void CargarDatos()
        {
            String opcion = comboBox_opciones.Text;

            if (opcion == "Compras") CargarInformacion("publicaciones.publi_descripcion Producto, compras.comp_cantidad Cantidad, publicaciones.publi_precio Precio, compras.comp_fecha Fecha, a_quien.usr_usuario 'A quien'", "NET_A_CERO.Compras compras, NET_A_CERO.Publicaciones publicaciones, NET_A_CERO.Usuarios a_quien", "compras.comp_publi_id = publicaciones.publi_id AND publicaciones.publi_usr_id = a_quien.usr_id AND compras.comp_usr_id = @idUsuario");
            if (opcion == "Ofertas") CargarInformacion("user1.usr_usuario 'De', user2.usr_usuario 'A quien', oferta.sub_monto 'Monto ofertado', oferta.sub_fecha 'Cuando oferto', publicacion.publi_descripcion 'Publicacion', oferta.sub_ganador 'Gano la subasta'", "NET_A_CERO.Ofertas_x_Subasta oferta, NET_A_CERO.Publicaciones publicacion, NET_A_CERO.Usuarios user1, NET_A_CERO.Usuarios user2", "oferta.sub_publi_id = publicacion.publi_id AND oferta.sub_usr_id = user1.usr_id AND publicacion.publi_usr_id = user2.usr_id AND (oferta.sub_usr_id = @idUsuario OR publicacion.publi_usr_id = @idUsuario)");
            if (opcion == "Calificaciones") CargarInformacion("user1.usr_usuario 'De', user2.usr_usuario 'A quien', calificacion.calif_cant_estrellas 'Estrellas', calificacion.calif_desc 'Descripcion calificacion', publicacion.publi_descripcion 'Publicacion', publicacion.publi_tipo 'Tipo de publicacion', compra.comp_fecha 'Cuando', compra.comp_cantidad 'Cuantos productos', (compra.comp_cantidad * publicacion.publi_precio) 'Monto pagado'", "NET_A_CERO.Compras compra, NET_A_CERO.Calificacion calificacion, NET_A_CERO.Publicaciones publicacion, NET_A_CERO.Usuarios user1, NET_A_CERO.Usuarios user2", "compra.comp_calif_id = calificacion.calif_id AND compra.comp_usr_id = user1.usr_id AND publicacion.publi_usr_id = user2.usr_id AND compra.comp_publi_id = publicacion.publi_id AND(compra.comp_usr_id = @idUsuario OR publicacion.publi_usr_id = @idUsuario)");
        }

        public void CargarInformacion(String select, String from, String where)
        {
            dataGridView_Historial.DataSource = mapper.SelectDataTableConUsuario(select, from, where);
            tablaTemporal = (DataTable)dataGridView_Historial.DataSource;
            
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
            dataGridView_Historial.DataSource = paginarDataGridView(ini, fin);
            dataGridView_Historial.Columns[0].Visible = false;
            mostrarNrosPaginas(ini);
            
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            comboBox_opciones.SelectedIndex = -1;
            labelNrosPagina.Text = "";

            //Limpio grilla siempre y cuando haya arrojado resultados
            if (dataGridView_Historial.DataSource != null)
            {
                DataTable dt = (DataTable)dataGridView_Historial.DataSource;
                dt.Clear();
                dataGridView_Historial.DataSource = null;
            }

            CargarDatos();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        /** Paginacion Events **/
        private void botonPrimeraPagina_Click(object sender, EventArgs e)
        {
            if (sePuedeRetrocederPaginas())
            {
                ini = 0;
                fin = 9;
                dataGridView_Historial.DataSource = paginarDataGridView(ini, fin);
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

                dataGridView_Historial.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
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
                dataGridView_Historial.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        private void botonUltimaPagina_Click(object sender, EventArgs e)
        {
            if (sePuedeAvanzarPaginas())
            {
                ini = (totalPaginas - 1) * publicacionesPorPagina;
                fin = totalPublicaciones;
                dataGridView_Historial.DataSource = paginarDataGridView(ini, fin);
                mostrarNrosPaginas(ini);
            }
        }

        /** Paginacion funciones **/
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
            DataTable datosDeUnaPagina = new DataTable();
            datosDeUnaPagina = tablaTemporal.Clone();
            for (int i = ini; i <= fin; i++)
            {
                datosDeUnaPagina.ImportRow(tablaTemporal.Rows[i]);
            }
            return datosDeUnaPagina;
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

    }
}