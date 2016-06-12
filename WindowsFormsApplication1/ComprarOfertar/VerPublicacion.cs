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
        private DBMapper mapper = new DBMapper();
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
            pedirRubroYPrecio();
            pedirVencimientoPreguntas();
            pedirStock();                        
                       
        }

        private void pedirTipoEstadoDescripcion()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";            
            
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();
            Decimal idEstado = Convert.ToInt32(reader["publi_estado_id"]);

            String estado = Convert.ToString(mapper.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstado));
            if (estado == "Pausada")
            {
                botonComprarOfertar.Enabled = false;
                MessageBox.Show("La publicación se encuentra pausada y no se pueden realizar compras/ofertas");
            }
            

            labelProductoDatos.Text = Convert.ToString(reader["publi_descripcion"]);
            tipoPublicacion = Convert.ToString(reader["publi_tipo"]);
            labelTipoDatos.Text = tipoPublicacion;
            
            
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

        private void pedirRubroYPrecio()
        {
            //Cargo Rubro
            Decimal rubroId = Convert.ToInt32(mapper.SelectFromWhere("rubro_id", "Rubro_x_Publicacion", "publi_id", publicacionId));
            labelRubroDatos.Text = Convert.ToString(mapper.SelectFromWhere("rubro_desc_larga", "Rubros", "rubro_id", rubroId));

            //Cargo Precio
            if (tipoPublicacion == "Compra inmediata")
            {
                labelPrecioDatos.Text = Convert.ToString(mapper.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));
                botonComprarOfertar.Text = "Comprar";


            }
            else
            {
                String precioSub = Convert.ToString(mapper.SelectFromWhere("vista_precioMax", "VistaOfertaMaxima", "vista_publi_id", publicacionId));
                botonComprarOfertar.Text = "Ofertar";
                if (precioSub == "")
                {
                   labelPrecioDatos.Text = Convert.ToString(mapper.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));
                }
                else
                {
                    labelPrecioDatos.Text = precioSub;
                }
            }



        }

        private void pedirVencimientoPreguntas()
        {
            
            labelVencimientoDatos.Text = Convert.ToString(mapper.SelectFromWhere("publi_fec_vencimiento", "Publicaciones", "publi_id", publicacionId));
                    
        }

        private void pedirStock()
        {
                     
           if (tipoPublicacion == "Subasta")
            {
                labelStockDatos.Text = Convert.ToString(mapper.SelectFromWhere("publi_stock", "Publicaciones", "publi_id", publicacionId));
                
            }
            else
            {
                Decimal stockInicial = Convert.ToInt32(mapper.SelectFromWhere("publi_stock", "Publicaciones", "publi_id", publicacionId));
                Decimal cantidadVendida = Convert.ToInt32(mapper.SelectFromWhere("vista_cant_vendida", "VistaCantidadVendida", "vista_publi_id", publicacionId));

                if (cantidadVendida == 0)
                {
                    labelStockDatos.Text = Convert.ToString(stockInicial);
                }
                else
                {
                    labelStockDatos.Text = Convert.ToString(stockInicial-cantidadVendida);
                }

            }
        }

        private void botonComprarOfertar_Click(object sender, EventArgs e)
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@user", UsuarioSesion.Usuario.id));
            String query = "SELECT COUNT(*) from NET_A_CERO.Compras c "
                + " WHERE ISNULL(c.comp_calif_id,0)=0 and c.comp_usr_id = @user";

            Decimal cantidad = Convert.ToDecimal(QueryBuilder.Instance.build(query, parametros).ExecuteScalar());

            if (cantidad >= 5)
            {
                MessageBox.Show("Tiene 5 compras sin haber calificado al vendedor. No puede realizar más compras hasta que no califique.");
                return;
            }

            if (tipoPublicacion == "Compra inmediata")
            {                
                this.Hide();
                new Comprar(vendedorId, publicacionId, Convert.ToInt32(labelStockDatos.Text)).ShowDialog();
                this.Close();
            }
            else
            {
                this.Hide();
                int precio = Convert.ToInt32(Convert.ToDecimal(labelPrecioDatos.Text)); 
                new Ofertar(precio, publicacionId).ShowDialog();
                this.Close();
            }
        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new BuscadorPublicaciones().ShowDialog();
            this.Close();
        }

    }
}
