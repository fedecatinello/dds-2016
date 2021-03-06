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
    public partial class Ofertar : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        decimal idUsuarioActual = UsuarioSesion.Usuario.id;
        private int ofertaMax;
        private int publicacionId;

        public Ofertar(int montoMax,int publicacion)
        {
            InitializeComponent();
            publicacionId = publicacion;
            ofertaMax = montoMax;
        }

        private void Ofertar_Load(object sender, EventArgs e)
        {

        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
                new VerPublicacion(publicacionId).ShowDialog();
                this.Close();
        }

        private void botonOfertar_Click(object sender, EventArgs e)
        {
            int val = 0;
            if (!Int32.TryParse(textBoxMonto.Text, out val))
            {
                MessageBox.Show("Solo puede ingresar un número entero");
                textBoxMonto.Clear();
                return;
            }
            
            if (Convert.ToInt32(this.textBoxMonto.Text) > ofertaMax)
            {
                String sql = "INSERT INTO NET_A_CERO.Ofertas_x_Subasta(sub_usr_id, sub_monto, sub_fecha, sub_ganador, sub_publi_id) VALUES (@sub_usr_id, @sub_monto, @sub_fecha, @sub_ganador, @sub_publi_id)";
                DateTime fecha = DateConfig.getInstance().getCurrentDate();
                parametros.Clear();
                parametros.Add(new SqlParameter("@sub_usr_id", idUsuarioActual));
                parametros.Add(new SqlParameter("@sub_monto", this.textBoxMonto.Text));
                parametros.Add(new SqlParameter("@sub_fecha", fecha));
                parametros.Add(new SqlParameter("@sub_ganador", 1)); //es el ganador actual por tener la oferta mayor a la ofertaMax
                parametros.Add(new SqlParameter("@sub_publi_id", publicacionId));
                
                QueryBuilder.Instance.build(sql, parametros).ExecuteNonQuery();                
                MessageBox.Show("Su oferta fue registrada");

                //Actualizamos los perdedores
                parametros.Clear();
                parametros.Add(new SqlParameter("@sub_publi_id", publicacionId));
                parametros.Add(new SqlParameter("@sub_usr_id", idUsuarioActual));
                String sqlUpdatePerdedores = "UPDATE NET_A_CERO.Ofertas_x_Subasta SET sub_ganador = 0 WHERE sub_publi_id = @sub_publi_id AND sub_usr_id <> @sub_usr_id";
                QueryBuilder.Instance.build(sqlUpdatePerdedores, parametros).ExecuteNonQuery();


                this.Hide();
                new VerPublicacion(publicacionId).ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Su oferta debe ser mayor a $" + ofertaMax);
                textBoxMonto.Clear();
            }
        }

        private void textBoxMonto_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {    
            int val = 0;
            if (!Int32.TryParse(textBoxMonto.Text, out val))
            {                
                e.Cancel = true;
            }            
        }
    }
}