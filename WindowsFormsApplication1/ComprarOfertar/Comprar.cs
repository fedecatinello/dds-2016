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
//        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        decimal idUsuarioActual = 4;
        private Decimal vendedorId;
        private int publicacionId;
        private int stockActual;
        private DBMapper comunicador = new DBMapper();

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
            
            String queryCliente = "SELECT * FROM NET_A_CERO.Clientes WHERE cli_usr_id= @usuario and cli_activo= 1";
            SqlDataReader readerCliente = QueryBuilder.Instance.build(queryCliente, parametros).ExecuteReader();

            int contId;

            if (readerCliente.Read()) //cliente
            {
                labelNombre.Text = Convert.ToString(readerCliente["cli_nombre"]) + " " + Convert.ToString(readerCliente["cli_apellido"]);
                labelDniCuit.Text = Convert.ToString(comunicador.SelectFromWhere("cli_tipo_dni", "Clientes", "cli_usr_id", vendedorId));
                labelNumDniCuit.Text = Convert.ToString(comunicador.SelectFromWhere("cli_dni", "Clientes", "cli_usr_id", vendedorId));
                contId = Convert.ToInt32(readerCliente["cli_cont_id"]);
                               
            }
            else        //empresa
            {
                parametros.Clear();
                parametros.Add(new SqlParameter("@usuario", vendedorId));
                String queryEmpresa = "SELECT * FROM NET_A_CERO.Empresas WHERE emp_usr_id = @usuario AND emp_activo = 1";
                SqlDataReader readerEmpresa = QueryBuilder.Instance.build(queryEmpresa, parametros).ExecuteReader();
                readerEmpresa.Read();
                labelNombre.Text = Convert.ToString(readerEmpresa["emp_razon_social"]);
                contId = Convert.ToInt32(readerEmpresa["emp_cont_id"]);

                labelDniCuit.Text = "CUIT";
                labelNumDniCuit.Text = Convert.ToString(comunicador.SelectFromWhere("emp_cuit", "Empresas", "emp_usr_id", vendedorId));


            }
            labelMail.Text = Convert.ToString(comunicador.SelectFromWhere("cont_mail", "Contacto", "cont_id", contId));
            labelTel.Text = Convert.ToString(comunicador.SelectFromWhere("cont_telefono", "Contacto", "cont_id", contId));
            labelCalle.Text = Convert.ToString(comunicador.SelectFromWhere("cont_calle", "Contacto", "cont_id", contId));
            labelNumCalle.Text = Convert.ToString(comunicador.SelectFromWhere("cont_numero_calle", "Contacto", "cont_id", contId));
            labelPiso.Text = Convert.ToString(comunicador.SelectFromWhere("cont_piso", "Contacto", "cont_id", contId));
            labelDepartamento.Text = Convert.ToString(comunicador.SelectFromWhere("cont_depto", "Contacto", "cont_id", contId));
            labelLocalidad.Text = Convert.ToString(comunicador.SelectFromWhere("cont_localidad", "Contacto", "cont_id", contId));
            labelPostal.Text = Convert.ToString(comunicador.SelectFromWhere("cont_codigo_postal", "Contacto", "cont_id", contId));
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

            String sql = "INSERT INTO NET_A_CERO.Compras(comp_cantidad, comp_fecha, comp_usr_id, comp_publi_id, comp_calif_id, comp_monto) VALUES (@cant, @fecha, @usuario, @publicacion, NULL,@monto)";
            //DateTime fecha = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["DateKey"]);
            DateTime fecha = DateConfig.getInstance().getCurrentDate();

            //CHEQUAR ESTA QUERY
            String sqlMonto = "SELECT publi_precio FROM NET_A_CERO.Publicaiones WHERE publi_id = @publicacion";
            parametros.Clear();
            parametros.Add(new SqlParameter("@cant",publicacionId));
            SqlDataReader readerMontoPublicacion = QueryBuilder.Instance.build(sqlMonto, parametros).ExecuteReader();
            readerMontoPublicacion.Read();
            Decimal precioPublicacion = ((Decimal)readerMontoPublicacion["publi_precio"]);

            parametros.Clear();
            parametros.Add(new SqlParameter("@cant", this.textBoxCant.Text));
            parametros.Add(new SqlParameter("@fecha", fecha));
            parametros.Add(new SqlParameter("@usuario", idUsuarioActual));
            parametros.Add(new SqlParameter("@publicacion", publicacionId));
            parametros.Add(new SqlParameter("@monto", (precioPublicacion * Convert.ToInt32(textBoxCant.Text))));
            QueryBuilder.Instance.build(sql, parametros).ExecuteNonQuery();

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

            Decimal idEstado = (Decimal)reader["publi_estado_id"];
            String estado = (String)comunicador.SelectFromWhere("estado_desc", "Estado", "estado_id", idEstado);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}