using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercadoEnvio.Calificar
{
    public partial class HistorialCalificaciones : Form
    {

        DBMapper mapper = new DBMapper();

        public HistorialCalificaciones()
        {
            InitializeComponent();
        }

        private void HistorialCalificaciones_Load(object sender, EventArgs e)
        {
            CargarUltimasCalificaciones();
            OcultarColumnasQueNoDebenVerse();
        }

        public void CargarUltimasCalificaciones()
        {
            dataGridViewHistorial.DataSource = mapper.SelectDataTableConUsuario("TOP 5 user1.usr_usuario 'De', user2.usr_usuario 'A quien', calificacion.calif_cant_estrellas 'Estrellas', calificacion.calif_desc 'Descripcion calificacion', publicacion.publi_descripcion 'Publicacion', publicacion.publi_tipo 'Tipo de publicacion', compra.comp_fecha 'Cuando', compra.comp_cantidad 'Cuantos productos', (compra.comp_cantidad * publicacion.publi_precio) 'Monto pagado'", "NET_A_CERO.Compras compra, NET_A_CERO.Calificacion calificacion, NET_A_CERO.Publicaciones publicacion, NET_A_CERO.Usuarios user1, NET_A_CERO.Usuarios user2", "compra.comp_calif_id = calificacion.calif_id AND compra.comp_usr_id = user1.usr_id AND publicacion.publi_usr_id = user2.usr_id AND compra.comp_publi_id = publicacion.publi_id AND(compra.comp_usr_id = @idUsuario OR publicacion.publi_usr_id = @idUsuario) ORDER BY compra.comp_fecha DESC");  
        }

        private void OcultarColumnasQueNoDebenVerse()
        {
            dataGridViewHistorial.Columns["De"].Visible = false;
        }

        private void buttonVolver_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Calificar_Vendedor.Listado().ShowDialog();
            this.Close();
        }
    }
}
