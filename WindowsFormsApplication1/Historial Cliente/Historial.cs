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
        private DBMapper comunicador = new DBMapper();

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
            dataGridView_Historial.DataSource = comunicador.SelectDataTableConUsuario(select, from, where);
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            comboBox_opciones.SelectedIndex = -1;
            CargarDatos();
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }
    }
}