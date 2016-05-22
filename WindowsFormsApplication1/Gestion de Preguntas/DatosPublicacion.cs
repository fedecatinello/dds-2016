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

namespace MercadoEnvio.Gestion_de_Preguntas
{
    public partial class DatosPublicacion : Form
    {
        private IList<SqlParameter> parametros = new List<SqlParameter>();        
        private int publicacionId;

        public DatosPublicacion(int idPublicacion)
        {
            InitializeComponent();
            publicacionId = idPublicacion;
        }

        private void DatosPublicacion_Load(object sender, EventArgs e)
        {
            pedirTipoEstadoDescripcion();
            pedirVencimiento();
            pedirStock();
            pedirPrecio();
        }

        private void pedirTipoEstadoDescripcion()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();
            
            labelPublicacionDatos.Text = (String)reader["descripcion"];
            labelTipoDatos.Text = (String)reader["tipo"];
            labelEstadoDatos.Text = (String)reader["estado"];
        }

        private void pedirVencimiento()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();

            labelVencimientoDatos.Text = ((DateTime)reader["fecha_vencimiento"]).ToString();            
        }

        private void pedirStock()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            if (labelTipoDatos.Text == "Subasta")
            {
                String querySubasta = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
                SqlDataReader readerSubasta = QueryBuilder.Instance.build(querySubasta, parametros).ExecuteReader();
                readerSubasta.Read();

                labelStockDatos.Text = ((Decimal)readerSubasta["stock"]).ToString();
            }
            else
            {
                String queryCompra = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
                SqlDataReader readerCompra = QueryBuilder.Instance.build(queryCompra, parametros).ExecuteReader();
                readerCompra.Read();
                Decimal stockInicial = (Decimal)readerCompra["stock"];

                parametros.Clear();
                parametros.Add(new SqlParameter("@id", publicacionId));
                String queryVista = "SELECT * FROM LOS_SUPER_AMIGOS.VistaCantidadVendida WHERE publicacion_id = @id";
                SqlDataReader readerVista = QueryBuilder.Instance.build(queryVista, parametros).ExecuteReader();

                if (readerVista.Read())
                {
                    labelStockDatos.Text = (stockInicial - (Decimal)readerVista["cant_vendida"]).ToString();
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

            if (labelTipoDatos.Text == "Compra Inmediata")
            {
                String queryCompra = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
                SqlDataReader readerCompra = QueryBuilder.Instance.build(queryCompra, parametros).ExecuteReader();
                readerCompra.Read();

                labelPrecioDatos.Text = ((Decimal)readerCompra["precio"]).ToString();
            }
            else
            {
                String queryVista = "SELECT * FROM LOS_SUPER_AMIGOS.VistaOfertaMax WHERE publicacion_id = @id";
                SqlDataReader readerVista = QueryBuilder.Instance.build(queryVista, parametros).ExecuteReader();
                
                if (readerVista.Read())
                {
                    labelPrecioDatos.Text = ((Decimal)readerVista["precioMax"]).ToString();
                }
                else
                {
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@id", publicacionId));
                    String queryOferta = "SELECT * FROM LOS_SUPER_AMIGOS.Publicacion WHERE id = @id";
                    SqlDataReader readerOferta = QueryBuilder.Instance.build(queryOferta, parametros).ExecuteReader();
                    readerOferta.Read();
                    labelPrecioDatos.Text = ((Decimal)readerOferta["precio"]).ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}