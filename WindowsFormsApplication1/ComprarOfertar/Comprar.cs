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
            pedirDireccion();            
        }

        private void pedirContacto()
        {
          //  String nombre = Convert.ToString(comunicador.SelectFromWhere("cli_nombre", "Clientes", "cli_usr_id", vendedorId));
          //  String apellido = Convert.ToString(comunicador.SelectFromWhere("cli_apellido", "Clientes", "cli_usr_id", vendedorId));

            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", vendedorId));

            String queryCliente = "SELECT * FROM NET_A_CERO.Clientes WHERE cli_usr_id= @usuario and (SELECT usr_activo FROM NET_A_CERO.Usuarios WHERE usr_id = @usuario) = 1";
            SqlDataReader readerCliente = QueryBuilder.Instance.build(queryCliente, parametros).ExecuteReader();

                     

            if (readerCliente.Read())
            {
                labelNombre.Text = (String)readerCliente["cli_nombre"] + " " + (String)readerCliente["cli_apellido"];
               
            }
            else
            {
                parametros.Clear();
                parametros.Add(new SqlParameter("@usuario", vendedorId));
                String queryEmpresa = "SELECT * FROM NET_A_CERO.Empresas WHERE usr_id = @usuario and (SELECT usr_activo FROM NET_A_CERO.Usuarios WHERE usr_id = @usuario) = 1";
                SqlDataReader readerEmpresa = QueryBuilder.Instance.build(queryEmpresa, parametros).ExecuteReader();
                readerEmpresa.Read();
                labelNombre.Text = (String)readerEmpresa["emp_razon_social"];
               
            }
        }

        private void pedirDireccion()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario", vendedorId));


            String queryContacto = "SELECT * FROM NET_A_CERO.Contacto WHERE cont_id = (SELECT cli_cont_id FROM NET_A_CERO.Clientes WHERE cli_id = @usuario)";
            SqlDataReader readerContacto = QueryBuilder.Instance.build(queryContacto, parametros).ExecuteReader();
            readerContacto.Read();

            labelMail.Text = (String)readerContacto["cont_mail"];
            if ((Decimal)readerContacto["cont_telefono"] == 0)
            {
                labelLocalidad.Text = "";
            }
            else
            {
                labelLocalidad.Text = ((Decimal)readerContacto["cont_telefono"]).ToString();
            }

            labelCalle.Text = (String)readerContacto["cont_calle"] + " " + (Decimal)readerContacto["cont_numero_calle"];
            labelDepartamento.Text = "Departamento " + (Decimal)readerContacto["cont_piso"] + "-" + (String)readerContacto["cont_depto"];
            labelPostal.Text = ((String)readerContacto["cont_codigo_postal"]).ToString();

            /*CHEQUEAR POR QUE EN LA MAESTRA NO HAY LOCALIDAD
            if ((String)readerContacto["cont_localidad"] == "localidadMigrada") 
            {
                labelLocalidad.Text = "";
            }
            else
            {*/
                labelLocalidad.Text = (String)readerContacto["cont_localidad"];
            //}
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
    }
}