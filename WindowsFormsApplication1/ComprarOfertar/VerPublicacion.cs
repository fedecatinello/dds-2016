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
    public partial class VerPublicacion : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private DBMapper comunicador = new DBMapper();
        public Object SelectedItem { get; set; }        
        private String tipoPublicacion;
        private int publicacionId;
        private Decimal vendedorId;

        public VerPublicacion(int idPublicacion)
        {
            InitializeComponent();            
            publicacionId = idPublicacion;
            pedirTipoEstadoDescripcion();
        }

        private void ComprarOfertar_Load(object sender, EventArgs e)
        {
            pedirVendedor();
            pedirRubro();
            pedirVencimientoPreguntas();
            pedirStock();                        
            pedirPrecio();
            pedirAccion();            
        }

        private void pedirTipoEstadoDescripcion()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";            
            
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();
            Decimal idEstado = Convert.ToInt32(reader["publi_estado_id"]);
            String estado = Convert.ToString(comunicador.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstado));
            if (estado == "Pausada")
            {
                botonComprarOfertar.Enabled = false;
                MessageBox.Show("La publicación se encuentra pausada y no se pueden realizar compras/ofertas");
            }
            

            labelProductoDatos.Text = Convert.ToString(reader["publi_descripcion"]);
            tipoPublicacion = Convert.ToString(reader["publi_tipo"]);
            
            
        }

        private void pedirVendedor()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));            

            String query = "SELECT * FROM NET_A_CERO.Usuarios WHERE usr_id = (SELECT publi_usr_id FROM NET_A_CERO.Publicaciones WHERE publi_id = @id)";

            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();
            vendedorId = Convert.ToInt32(reader["usr_id"]);
            labelVendedorDatos.Text = Convert.ToString(reader["usr_usuario"]);            
        }

        private void pedirRubro()
        {

            Decimal rubroId = Convert.ToInt32(comunicador.SelectFromWhere("rubro_id", "Rubro_x_Publicacion", "publi_id", publicacionId));
            labelRubroDatos.Text = Convert.ToString(comunicador.SelectFromWhere("rubro_desc_larga", "Rubros", "rubro_id", rubroId));

            
        }

        private void pedirVencimientoPreguntas()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";

            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();
            labelVencimientoDatos.Text = ( Convert.ToDateTime(reader["publi_fec_vencimiento"] ).ToString());
            
            if ((int)reader["publi_preguntas"] == 0)
            {
                botonPreguntar.Enabled = false;
            }            
        }

        private void pedirStock()
        {
            labelStockDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_stock", "Publicaciones", "publi_id", publicacionId));
            
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));
            
           
            if (tipoPublicacion == "Subasta")
            {
                labelStockDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_stock", "Publicaciones", "publi_id", publicacionId));
                
            }
            else
            {
                String queryCompra = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";
                SqlDataReader readerCompra = QueryBuilder.Instance.build(queryCompra, parametros).ExecuteReader();
                readerCompra.Read();
              
                Decimal stockInicial = (Convert.ToInt32(readerCompra["publi_stock"]));

                parametros.Clear();
                parametros.Add(new SqlParameter("@id", publicacionId));
                String queryVista = "SELECT * FROM NET_A_CERO.VistaCantidadVendida WHERE vista_publi_id = @id";  //CHEQUEAR POR LA VISTA
                SqlDataReader readerVista = QueryBuilder.Instance.build(queryVista, parametros).ExecuteReader();//CHEQUEAR POR LA VISTA

                if (readerVista.Read()) //CHEQUEAR POR LA VISTA                                         
                {
                    
                    labelStockDatos.Text = (stockInicial - (Decimal)readerVista["vista_cant_vendida"]).ToString();
                }
                else
                {                    
                    labelStockDatos.Text = stockInicial.ToString();
                }
            }
        }

        private void pedirPrecio()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));
            labelPrecioDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));
            if (tipoPublicacion == "Compra inmediata")
            {   /*
                String queryCompra = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";
                SqlDataReader readerCompra = QueryBuilder.Instance.build(queryCompra, parametros).ExecuteReader();
                readerCompra.Read();
                labelPrecioDatos.Text = ( Convert.ToDecimal(readerCompra["publi_precio"] )).ToString();*/
                labelPrecioDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));
            }
            else
            {
                labelPrecioDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_stock", "VistaOfertaMaxima", "publi_id", publicacionId));
                labelPrecioDatos.Text = Convert.ToString(comunicador.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));
                /*
                String queryVista = "SELECT * FROM NET_A_CERO.VistaOfertaMaxima WHERE vista_publi_id = @id"; //CHEQUEAR POR LA VISTA
                SqlDataReader readerVista = QueryBuilder.Instance.build(queryVista, parametros).ExecuteReader();//CHEQUEAR POR LA VISTA
                if (readerVista.Read())//CHEQUEAR POR LA VISTA
                {
                    labelPrecioDatos.Text = (Convert.ToDecimal(readerVista["vista_precioMax"])).ToString();
                }
                else
                {
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@id", publicacionId));
                    String queryOferta = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";
                    SqlDataReader readerOferta = QueryBuilder.Instance.build(queryOferta, parametros).ExecuteReader();
                    readerOferta.Read();
                    labelPrecioDatos.Text = ((Decimal)readerOferta["publi_precio"]).ToString();
                }*/
            }
        }

        private void pedirAccion()
        {
            if (tipoPublicacion == "Compra inmediata")
            {
                botonComprarOfertar.Text = "Comprar";
            }
            else
            {
                botonComprarOfertar.Text = "Ofertar";
            }
        }               

        private void botonComprarOfertar_Click(object sender, EventArgs e)
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@user", UsuarioSesion.Usuario.id));
            String query = "select COUNT(*) from NET_A_CERO.Compras c "
                + "where isnull(c.comp_calif_id,0)=0 and c.comp_usr_id = @user";
            Decimal cantidad = Convert.ToDecimal(QueryBuilder.Instance.build(query, parametros).ExecuteScalar());

            if (cantidad >= 5)
            {
                MessageBox.Show("Tiene 5 compras sin haber calificado al vendedor. No puede realizar más compras hasta que no califique.");
                return;
            }

            if (tipoPublicacion == "Compra Inmediata")
            {                
                this.Hide();
                new Comprar(vendedorId, publicacionId, Convert.ToInt32(labelStockDatos.Text)).ShowDialog();
                this.Close();
            }
            else
            {
                this.Hide();
                new Ofertar(Convert.ToInt32(labelPrecioDatos.Text),publicacionId).ShowDialog();
                this.Close();
            }
        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new BuscadorPublicaciones().ShowDialog();
            this.Close();
        }

        private void botonPreguntar_Click(object sender, EventArgs e)
        {
            new Preguntar(publicacionId).ShowDialog();
        }
    }
}
