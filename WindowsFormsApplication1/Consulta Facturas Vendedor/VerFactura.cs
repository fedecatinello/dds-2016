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
    public partial class VerFactura : Form
    {

        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();   
        private int facturaId;

        public VerFactura(int idFactura)
        {
            InitializeComponent();
            facturaId = idFactura;
            CargarDatosFactura();
        }

        public void CargarDatosFactura()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", facturaId));

            String query = "SELECT * FROM NET_A_CERO.Facturas WHERE fact_id = @id";

            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();

            label_Id.Text = Convert.ToString(facturaId);
            labelFechaDatos.Text = Convert.ToString(reader["fact_fecha"]);
            labelMontoDatos.Text = Convert.ToString(reader["fact_monto"]);
            labelFormaPagoDatos.Text = Convert.ToString(reader["fact_forma_pago"]);

            LlenarPublicacionFactura(Convert.ToDecimal(reader["fact_publi_id"]));

            LlenarItemsFactura();

        }

        public void LlenarPublicacionFactura(Decimal idPubli)
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", idPubli));
            String query = "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";

            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            reader.Read();

            labelPublicacionDatos.Text = Convert.ToString(reader["publi_descripcion"]);
        }

        public void LlenarItemsFactura()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@idFactura", facturaId));
            String query = "SELECT item_tipo 'Tipo Item', item_cantidad 'Cantidad', item_monto 'Monto' FROM NET_A_CERO.Items WHERE item_fact_id = @idFactura";
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandTimeout = 0;
            DataSet datos = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(datos);
            dataGridViewItems.DataSource = datos.Tables[0];
        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ListadoFacturas().ShowDialog();
            this.Close();
        }

    }
}
