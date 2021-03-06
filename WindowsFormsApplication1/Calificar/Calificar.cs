using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.DataProvider;

namespace MercadoEnvio.Calificar_Vendedor
{
    public partial class Calificar : Form
    {
        private SqlCommand command { get; set; }
        private IList<SqlParameter> parametros = new List<SqlParameter>();
       
        private Decimal id;
        private String tipo;
        private int calificacion { get; set; }
        private string descripcion { get; set; }

        public Calificar(Decimal idCompraParaCalificar)
        {
            InitializeComponent();
            id = idCompraParaCalificar;
            calificacion = 0;
            descripcion = "";
        }

        private void Calificar_Load(object sender, EventArgs e)
        {
            dropdownCalificacion.Items.Add(5);
            dropdownCalificacion.Items.Add(4);
            dropdownCalificacion.Items.Add(3);
            dropdownCalificacion.Items.Add(2);
            dropdownCalificacion.Items.Add(1);
            dropdownCalificacion.Items.Add(0);
            
            checkBoxPredeterminado.Checked = true;

            comboBoxDescripciones.Items.Add("Vendedor muy confiable");
            comboBoxDescripciones.Items.Add("Cumplio con lo prometido");
            comboBoxDescripciones.Items.Add("No estoy completamente satisfecho con el vendedor");
            comboBoxDescripciones.Items.Add("No le compren cosas a esta persona");
        }

        private void checkBoxPredeterminado_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPredeterminado.Checked)
            {
                comboBoxDescripciones.Enabled = true;
                textBoxDescripcion.Enabled = false;
            }
            else
            {
                comboBoxDescripciones.Enabled = false;
                textBoxDescripcion.Enabled = true;
            }
        }

        private void botonCalificar_Click(object sender, EventArgs e)
        {
            if (checkBoxPredeterminado.Checked)
            {
                if (comboBoxDescripciones.SelectedItem != null)
                {
                    descripcion = comboBoxDescripciones.SelectedItem.ToString();
                }
            }
            else
            {
                descripcion = textBoxDescripcion.Text;
            }

            if (this.calificacion == 0)
            {
                MessageBox.Show("Seleccione una cantidad de estrellas");
                return;
            }

            parametros.Add(new SqlParameter("@estrellas", this.calificacion));
            parametros.Add(new SqlParameter("@descripcion", descripcion));

            // inserta nueva calificacion
            String nuevaCalificacion = "insert NET_A_CERO.Calificacion"
                                + " (calif_cant_estrellas, calif_desc)"
                                + " values(@estrellas,@descripcion)";
            QueryBuilder.Instance.build(nuevaCalificacion, parametros).ExecuteNonQuery();

            parametros.Clear();
            String idCalificacion = "select top 1 calif_id"
                                + " from NET_A_CERO.Calificacion"
                                + " order by calif_id DESC";
            Decimal elId = (Decimal)QueryBuilder.Instance.build(idCalificacion, parametros).ExecuteScalar();

            parametros.Clear();
            parametros.Add(new SqlParameter("@idCalif", elId));
            parametros.Add(new SqlParameter("@id", id));

            // referencia en compra a la calificacion

            String compraAct = "update NET_A_CERO.Compras"
                             + " set comp_calif_id = @idCalif"
                             + " where comp_id = @id";
            QueryBuilder.Instance.build(compraAct, parametros).ExecuteNonQuery();


            MessageBox.Show("Calificacion hecha correctamente");

            this.Hide();
            new Listado().ShowDialog();
            this.Close();
        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Listado().ShowDialog();
            this.Close();
        }

        private void dropdownCalificacion_SelectedItemChanged(object sender, EventArgs e)
        {
            calificacion = (int)dropdownCalificacion.SelectedItem;
        }

    }
}