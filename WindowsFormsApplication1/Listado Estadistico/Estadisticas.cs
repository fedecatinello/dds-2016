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


namespace MercadoEnvio.Listado_Estadistico
{
    public partial class Estadisticas : Form
    {
        private DBMapper comunicador = new DBMapper();
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private SqlCommand command;

        public Estadisticas()
        {
            InitializeComponent();
        }

        private void Estadisticas_Load(object sender, EventArgs e)
        {
            CargarTrimestres();
            CargarTiposDeListados();
            progressBar.Visible = false;
        }

        private void CargarTrimestres()
        {
            DataTable trimestres = new DataTable();
            trimestres.Columns.Add("trimestres");
            trimestres.Rows.Add("1er trimestre (Enero - Marzo)");
            trimestres.Rows.Add("2do trimestre (Abril - Junio)");
            trimestres.Rows.Add("3er trimestre (Julio - Septiembre)");
            trimestres.Rows.Add("4to trimestre (Octubre - Diciembre)");
            comboBox_Trimestre.DataSource = trimestres;
            comboBox_Trimestre.ValueMember = "trimestres";
            comboBox_Trimestre.SelectedIndex = -1;
        }

        private void CargarTiposDeListados()
        {
            DataTable tiposDeListados = new DataTable();
            tiposDeListados.Columns.Add("tiposDeListados");
            tiposDeListados.Rows.Add("Vendedores con mayor cantidad de productos no vendidos");
            tiposDeListados.Rows.Add("Vendedores con mayor facturacion");
            tiposDeListados.Rows.Add("Vendedores con mayores calificaciones");
            tiposDeListados.Rows.Add("Clientes con mayor cantidad de publicaciones sin calificar");
            comboBox_TipoDeListado.DataSource = tiposDeListados;
            comboBox_TipoDeListado.ValueMember = "tiposDeListados";
            comboBox_TipoDeListado.SelectedIndex = -1;
        }

        private void button_Buscar_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 1000;

            String anio = textBox_Anio.Text;
            String trimestre = comboBox_Trimestre.Text;
            String tipoDeListado = comboBox_TipoDeListado.Text;

            String fechaDeInicio = ObtenerFechaDeInicio(anio, trimestre);
            String fechaDeFin = ObtenerFechaDeFin(anio, trimestre);
            String fechaMedia = ObtenerFechaMedia(anio, trimestre);

            if (tipoDeListado == "Vendedores con mayor cantidad de productos no vendidos")
            {
                progressBar.Visible = true;
                progressBar.Value = 50;
                String borrar = "IF OBJECT_ID('NET_A_CERO.usuarios_por_visibilidad', 'U') IS NOT NULL"
                            + " DROP TABLE NET_A_CERO.usuarios_por_visibilidad";
                parametros.Clear();
                QueryBuilder.Instance.build(borrar, parametros).ExecuteNonQuery();

                String crearTabla = "CREATE TABLE NET_A_CERO.usuarios_por_visibilidad"
                                    + " (mes int,"
                                    + " visibilidad nvarchar(255),"
                                    + "	usuario nvarchar(50),"
                                    + " cantidad numeric(18,0),"
                                    + " PRIMARY KEY(mes, visibilidad, usuario))";
                parametros.Clear();
                QueryBuilder.Instance.build(crearTabla, parametros).ExecuteNonQuery();
                progressBar.Value = 500;
                String fillTable = "DECLARE mi_cursor CURSOR FOR"
                                + " SELECT DATEPART(month, fecha) Mes, visibilidad.visib_desc "
                                + " FROM (VALUES(@fechaini), (@fechamed), (@fechafin)) as F(fecha), NET_A_CERO.Visibilidad visibilidad"
                                + " ORDER BY Mes, visibilidad.visib_id"
                                + " DECLARE @mes int, @visibilidad nvarchar(255)"
                                + " OPEN mi_cursor"
                                + " FETCH FROM mi_cursor INTO @mes, @visibilidad"
                                + " WHILE  @@FETCH_STATUS = 0"
                                + " BEGIN"
                                + " INSERT INTO NET_A_CERO.usuarios_por_visibilidad ([mes], [visibilidad], [usuario], [cantidad])"
                                + " SELECT TOP 5 @mes, @visibilidad, usuario.usr_usuario, NET_A_CERO.calcular_productos_no_vendidos(usuario.usr_id, (@visibilidad), (@fechaini), (@fechafin)) Cantidad"
                                + " FROM NET_A_CERO.Usuario usuario"
                                + " ORDER BY Cantidad DESC"
                                + " FETCH FROM mi_cursor INTO @mes, @visibilidad"
                                + " END"
                                + " CLOSE mi_cursor"
                                + " DEALLOCATE mi_cursor";
                parametros.Clear();
                parametros.Add(new SqlParameter("@fechaini", Convert.ToDateTime(fechaDeInicio)));
                parametros.Add(new SqlParameter("@fechamed", Convert.ToDateTime(fechaMedia)));
                parametros.Add(new SqlParameter("@fechafin", Convert.ToDateTime(fechaDeFin)));
                command = QueryBuilder.Instance.build(fillTable, parametros);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                progressBar.Value = 1000;
                parametros.Clear();
                command = QueryBuilder.Instance.build("SELECT  u.mes, u.visibilidad, u.usuario, u.cantidad FROM NET_A_CERO.usuarios_por_visibilidad u, NET_A_CERO.Visibilidad visibilidad WHERE u.visibilidad = visibilidad.visib_desc ORDER BY u.mes, visibilidad.visib_precio DESC, u.cantidad DESC", parametros);
                DataSet datos = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(datos);
                dataGridView_Estadistica.DataSource = datos.Tables[0];
                return;
            }
            progressBar.Visible = false;
            String queryParaObtenerResultados = GetQueryObtenerResultados(tipoDeListado, fechaDeInicio, fechaMedia, fechaDeFin);
            
            dataGridView_Estadistica.DataSource = comunicador.SelectDataTable("*", queryParaObtenerResultados);
        }

        private String ObtenerFechaMedia(string anio, string trimestre)
        {
            String dia = "01";
            String mes = ObtenerMesMedio(trimestre);
            return dia + "/" + mes + "/" + anio;
        }

        private string ObtenerMesMedio(string trimestre)
        {
            switch (trimestre[0])
            {
                case '1':
                    return "02";
                case '2':
                    return "05";
                case '3':
                    return "08";
                case '4':
                    return "11";
            }
            throw new Exception("No se pudo obtener el mes");
        }

        private String ObtenerFechaDeInicio(string anio, string trimestre)
        {
            String dia = "01";
            String mes = ObtenerMesInicio(trimestre);
            return dia + "/" + mes + "/" + anio;
        }

        private string ObtenerMesInicio(string trimestre)
        {
            switch (trimestre[0])
            {
                case '1':
                    return "01"; //Enero
                case '2':
                    return "04"; //Abril
                case '3':
                    return "07"; //Julio
                case '4':
                    return "10"; //Octubre
            }
            throw new Exception("No se pudo obtener el mes");
        }

        private String ObtenerFechaDeFin(string anio, string trimestre)
        {
            String dia = "01";
            String mes = ObtenerMesFin(trimestre);
            return dia + "/" + mes + "/" + anio;
        }

        private string ObtenerMesFin(string trimestre)
        {
            switch (trimestre[0])
            {
                case '1':
                    return "03"; //Marzo
                case '2':
                    return "06"; //Junio
                case '3':
                    return "09"; //Septiembre
                case '4':
                    return "12"; //Diciembre
            }
            throw new Exception("No se pudo obtener el mes");
        }

        private string GetQueryObtenerResultados(String tipoDeListado, String fechaDeInicio, String fechaMedia, String fechaDeFin)
        {
            switch (tipoDeListado)
            {
              //  case "Vendedores con mayor cantidad de productos no vendidos":
               //     return "NET_A_CERO.vendedores_con_mayor_cantidad_de_publicaciones_sin_vender('" + fechaDeInicio + "', '" + fechaMedia + "' , '" + fechaDeFin + "')";
                case "Vendedores con mayor facturacion":
                    return "NET_A_CERO.vendedores_con_mayor_facturacion('" + fechaDeInicio + "', '" + fechaDeFin + "')";
                case "Vendedores con mayores calificaciones":
                    return "NET_A_CERO.vendedores_con_mayor_calificacion('" + fechaDeInicio + "', '" + fechaDeFin + "')";
                case "Clientes con mayor cantidad de publicaciones sin calificar":
                    return "NET_A_CERO.clientes_con_publicaciones_sin_calificar('" + fechaDeInicio + "', '" + fechaDeFin + "')";
            }
            throw new Exception("No se pudo obtener la funcion");
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_Anio.Text = "";
            comboBox_Trimestre.SelectedIndex = -1;
            comboBox_TipoDeListado.SelectedIndex = -1;
            dataGridView_Estadistica.DataSource = null;
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void textBox_Anio_TextChanged(object sender, EventArgs e)
        {
            String anio = textBox_Anio.Text;
            if (esNumero(anio) && longitudValida(anio))
            {
                comboBox_Trimestre.Enabled = true;
                return;
            }
            comboBox_Trimestre.Enabled = false;
            comboBox_Trimestre.SelectedIndex = -1;
        }

        private Boolean esNumero(String anio)
        {
            UInt32 num;
            return UInt32.TryParse(anio, out num);  
        }

        private Boolean longitudValida(String anio)
        {
            return anio.Length == 4;
        }

        private void comboBox_Trimestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Trimestre.SelectedIndex == -1)
            {
                comboBox_TipoDeListado.Enabled = false;
                comboBox_TipoDeListado.SelectedIndex = -1;
                return;
            }
            comboBox_TipoDeListado.Enabled = true;
        }

        private void comboBox_TipoDeListado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_TipoDeListado.SelectedIndex == -1)
            {
                button_Buscar.Enabled = false;
                return;
            }                
            button_Buscar.Enabled = true;
        }
    }
}