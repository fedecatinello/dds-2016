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
    public partial class Comprar : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        private Decimal vendedorId;
        private int publicacionId;
        private int stockActual;
        private DBMapper mapper = new DBMapper();

        public Comprar(Decimal usuarioVendedor, int publicacion, int stock)
        {
            InitializeComponent();
            vendedorId = usuarioVendedor;
            publicacionId = publicacion;
            stockActual = stock;
        }

        private void Comprar_Load(object sender, EventArgs e)
        {
            pedirContacto();
            
        }

         private void pedirContacto()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", vendedorId));

            String queryCliente = "SELECT * FROM NET_A_CERO.Clientes WHERE cli_usr_id = @usuario and cli_activo = 1";
            SqlDataReader readerCliente = QueryBuilder.Instance.build(queryCliente, parametros).ExecuteReader();

            int contId;

            if (readerCliente.Read()) //cliente
            {
               labelNombre.Text = Convert.ToString(readerCliente["cli_nombre"]) + " " + Convert.ToString(readerCliente["cli_apellido"]);
               labelDniCuit.Text = Convert.ToString(mapper.SelectFromWhere("cli_tipo_dni", "Clientes", "cli_usr_id", vendedorId));
               labelNumDniCuit.Text = Convert.ToString(mapper.SelectFromWhere("cli_dni", "Clientes", "cli_usr_id", vendedorId));
               contId = Convert.ToInt32(readerCliente["cli_cont_id"]);
               
            }
            else //empresa
            {
                parametros.Clear();
                parametros.Add(new SqlParameter("@usuario", vendedorId));
                String queryEmpresa = "SELECT * FROM NET_A_CERO.Empresas WHERE emp_usr_id = @usuario AND emp_activo = 1";
                SqlDataReader readerEmpresa = QueryBuilder.Instance.build(queryEmpresa, parametros).ExecuteReader();
                readerEmpresa.Read();
                
                labelNombre.Text = Convert.ToString(readerEmpresa["emp_razon_social"]);
                contId = Convert.ToInt32(readerEmpresa["emp_cont_id"]);
  
                labelDniCuit.Text = "CUIT";
                labelNumDniCuit.Text = Convert.ToString(mapper.SelectFromWhere("emp_cuit", "Empresas", "emp_usr_id", vendedorId));

            
            }
               labelMail.Text = Convert.ToString(mapper.SelectFromWhere("cont_mail", "Contacto", "cont_id", contId));
               labelTel.Text = Convert.ToString(mapper.SelectFromWhere("cont_telefono", "Contacto", "cont_id", contId));
               labelCalle.Text = Convert.ToString(mapper.SelectFromWhere("cont_calle", "Contacto", "cont_id", contId));
               labelNumCalle.Text = Convert.ToString(mapper.SelectFromWhere("cont_numero_calle", "Contacto", "cont_id", contId));
               labelPiso.Text = Convert.ToString(mapper.SelectFromWhere("cont_piso", "Contacto", "cont_id", contId));
               labelDepartamento.Text = Convert.ToString(mapper.SelectFromWhere("cont_depto", "Contacto", "cont_id", contId));
               labelLocalidad.Text = Convert.ToString(mapper.SelectFromWhere("cont_localidad", "Contacto", "cont_id", contId));
               labelPostal.Text = Convert.ToString(mapper.SelectFromWhere("cont_codigo_postal", "Contacto", "cont_id", contId));
         
            

        }

        private void buttonConfirmarCompra_Click(object sender, EventArgs e)
        {
            uint val = 0;
            if (!UInt32.TryParse(textBoxCant.Text, out val))
            {
                MessageBox.Show("Solo puede ingresar un número entero positivo");
                textBoxCant.Clear();
                return;
            }

            if (Convert.ToInt32(textBoxCant.Text) == 0)
            {
                MessageBox.Show("No puede hacer un pedido por 0 unidades");
                return;
            }

            if (Convert.ToInt32(textBoxCant.Text) > stockActual)
            {
                MessageBox.Show("Su pedido excede el stock actual de " + stockActual + " unidades");
                return;
            }

            Decimal cantidadVentas = Convert.ToDecimal(mapper.SelectFromWhere("COUNT(*)", "Compras", "comp_publi_id", publicacionId));

            String sql = "INSERT INTO NET_A_CERO.Compras(comp_usr_id, comp_publi_id, comp_fecha, comp_cantidad, comp_monto, comp_calif_id) VALUES (@comp_usr_id, @comp_publi_id, @comp_fecha, @comp_cantidad, @comp_monto, NULL)" ;
            DateTime fecha = DateConfig.getInstance().getCurrentDate();

            String sqlMonto = "SELECT publi_precio FROM NET_A_CERO.Publicaciones WHERE publi_id = @publicacion";
            parametros.Clear();
            parametros.Add(new SqlParameter("@publicacion", publicacionId));
            SqlDataReader readerMontoPublicacion = QueryBuilder.Instance.build(sqlMonto, parametros).ExecuteReader();
            readerMontoPublicacion.Read();
            Decimal precioPublicacion = Convert.ToDecimal(readerMontoPublicacion["publi_precio"]);

            parametros.Clear();
            parametros.Add(new SqlParameter("@comp_usr_id", idUsuarioActual));
            parametros.Add(new SqlParameter("@comp_publi_id", publicacionId));
            parametros.Add(new SqlParameter("@comp_fecha", fecha));
            parametros.Add(new SqlParameter("@comp_cantidad", this.textBoxCant.Text));
            parametros.Add(new SqlParameter("@comp_monto", (precioPublicacion * Convert.ToInt32(textBoxCant.Text))));
            QueryBuilder.Instance.build(sql, parametros).ExecuteNonQuery();

            

            Decimal precio = Convert.ToDecimal(mapper.SelectFromWhere("publi_precio", "Publicaciones", "publi_id", publicacionId));

            if (cantidadVentas == 0)
            {
                //Obtener ultima factura
                String idFactura = "select top 1 f.fact_id from NET_A_CERO.Facturas f order by f.fact_id DESC";
                parametros.Clear();
                Decimal idFact = Convert.ToDecimal(QueryBuilder.Instance.build(idFactura, parametros).ExecuteScalar());

                //Inserto facutra

                String insertarFactura = " INSERT INTO NET_A_CERO.Facturas (fact_id, fact_fecha, fact_monto, fact_destinatario, fact_forma_pago, fact_publi_id) " +
                    " VALUES(@fact_id, @fact_fecha, @fact_monto, @fact_destinatario, @fact_forma_pago, @fact_publi_id ) ";
                parametros.Clear();
                parametros.Add(new SqlParameter("@fact_id", idFact + 1));
                parametros.Add(new SqlParameter("@fact_fecha", DateConfig.getInstance().getCurrentDate()));
                parametros.Add(new SqlParameter("@fact_monto", precio));
                parametros.Add(new SqlParameter("@fact_destinatario", UsuarioSesion.Usuario.id));
                parametros.Add(new SqlParameter("@fact_forma_pago", "Efectivo"));
                parametros.Add(new SqlParameter("@fact_publi_id", publicacionId));
                command = QueryBuilder.Instance.build(insertarFactura, parametros);
                command.ExecuteNonQuery();

                String insertarItems = "INSERT INTO NET_A_CERO.Items (item_cantidad, item_tipo, item_monto, item_fact_id)" +
                        " VALUES (@item_cantidad, @item_tipo, @item_monto,@item_fact_id ) ";
                parametros.Clear();
                parametros.Add(new SqlParameter("@item_cantidad", Convert.ToInt32(textBoxCant.Text)));
                parametros.Add(new SqlParameter("@item_tipo", "Compra"));
                parametros.Add(new SqlParameter("@item_monto", precio));
                parametros.Add(new SqlParameter("@item_fact_id", Convert.ToDecimal(idFact +1)));
                command = QueryBuilder.Instance.build(insertarItems, parametros);
                command.ExecuteNonQuery();

            }
            else
            {
                //Obtener misma factura de la Publicacion
                Decimal facturaID = Convert.ToDecimal(mapper.SelectFromWhere("MAX(fact_id)", "Facturas", "fact_publi_id", publicacionId));

                String insertarItems = "INSERT INTO NET_A_CERO.Items (item_cantidad, item_tipo, item_monto, item_fact_id)" +
                        " VALUES (@item_cantidad, @item_tipo, @item_monto,@item_fact_id ) " +
                        " UPDATE NET_A_CERO.Facturas SET fact_monto = fact_monto + @item_monto WHERE fact_id = @item_fact_id ";
                parametros.Clear();
                parametros.Add(new SqlParameter("@item_cantidad", Convert.ToInt32(textBoxCant.Text)));
                parametros.Add(new SqlParameter("@item_tipo", "Compra"));
                parametros.Add(new SqlParameter("@item_monto", precio));
                parametros.Add(new SqlParameter("@item_fact_id", facturaID));
                command = QueryBuilder.Instance.build(insertarItems, parametros);
                command.ExecuteNonQuery();

            }


            MessageBox.Show("Contactese con el vendedor para finalizar la compra");
            if (pedirEstado())
            {
                this.Hide();
                new VerPublicacion(publicacionId).ShowDialog();
                this.Close();
            }
            else
            {
                this.Hide();
                new BuscadorPublicaciones().ShowDialog();
                this.Close();
            }
             
        }

         private bool pedirEstado()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", publicacionId));

            String query = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();

            Decimal idEstado = Convert.ToDecimal(reader["publi_estado_id"]);
            String estado = Convert.ToString(mapper.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstado));
            if (estado == "Finalizada")
            {
                return false;
            }
            else
            {
                return true;
            }
        } 

        private void botonCancelar_Click(object sender, EventArgs e)
        {
             this.Hide();
            new VerPublicacion(publicacionId).ShowDialog();
            this.Close();
        }
    }
}