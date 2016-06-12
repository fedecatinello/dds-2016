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

namespace MercadoEnvio.Facturar_Publicaciones
{
    public partial class Facturar : Form
    {
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        int cantidadMin;
        int cantidadMax;
        private SqlCommand command;
        private String formaDePago;

        public Facturar()
        {
            InitializeComponent();
        }

        private void Facturar_Load(object sender, EventArgs e)
        {
           CargarCostosPublicacionPorFacturar();
           CargarComisionesVentasPorFacturar();
           CalcularMonto();
           radioButtonEfectivo.Checked = true;
           textBoxBanco.Enabled = false;
           textBoxNumero.Enabled = false;
        }

        // Calculo el monto a pagar en la factura
        private void CalcularMonto()
        {

            // Borro tabla temporal con ventas que se facturan pagando comision
            String borroTabla = "IF OBJECT_ID('NET_A_CERO.Facturas_temporarias', 'U') IS NOT NULL drop table NET_A_CERO.Facturas_temporarias";
            parametros.Clear();
            QueryBuilder.Instance.build(borroTabla, parametros).ExecuteNonQuery();
            int valor;
            Int32.TryParse(dropDownFacturar.Text, out valor);

            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));
            parametros.Add(new SqlParameter("@cant", valor));

            String monto = "create table NET_A_CERO.Facturas_temporarias"
                        + " (compra_id numeric(18,0),"
                        + " fact_fecha datetime,"
                        + " fact_publi_id numeric(18,0),"
                        + " fact_monto numeric(18,2))"
                        + " insert into NET_A_CERO.Facturas_temporarias"
                        + " (compra_id, fact_fecha, fact_publi_id, fact_monto)"
                        + " select top (@cant) c.comp_id, c.comp_fecha, c.comp_publi_id, c.comp_cantidad"
                        + " from NET_A_CERO.Usuarios u, NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p"
                        + " where u.usr_id = @id and p.publi_usr_id = u.usr_id and c.comp_publi_id = p.publi_id"
                        + " order by c.comp_fecha"
                        + " select (select  isnull( (select sum(c.comp_cantidad * p.publi_precio * v.visib_porcentaje)"
                        + " from NET_A_CERO.Compras c,"
                        + " NET_A_CERO.Publicaciones p, NET_A_CERO.Visibilidad v"
                        + " where c.comp_publi_id = p.publi_id and p.publi_visib_id = v.visib_id),0))"
                        + " + (select isnull( (select sum(v.visib_precio)"
                        + " from NET_A_CERO.Publicaciones p, NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u"
                        + " where p.publi_costo_pagado = 0 and p.publi_visib_id = v.visib_id and p.publi_usr_id = u.usr_id and u.usr_id = @id"
                        + " and p.publi_estado_id = (select e.estado_id from NET_A_CERO.Estado e where e.estado_desc = 'Finalizada')),0))";
            Double montoCalculado = Convert.ToDouble(QueryBuilder.Instance.build(monto, parametros).ExecuteScalar());
            labelMontoCalculado.Text = montoCalculado.ToString();


            // Borro tabla temporal con monto
            String borroTablaTemporal = "drop table NET_A_CERO.Facturas_temporarias";
            parametros.Clear();
            QueryBuilder.Instance.build(borroTablaTemporal, parametros).ExecuteNonQuery();
        }

        private void CargarCostosPublicacionPorFacturar()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));

            String cantidadCostos = "select COUNT(p.publi_id) from NET_A_CERO.Publicaciones p,"
            + " NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u"
            + " where p.publi_usr_id = u.usr_id and u.usr_id = @id and p.publi_visib_id = v.visib_id"
            + " and p.publi_costo_pagado = 0 and p.publi_estado_id = (select estado_id from NET_A_CERO.Estado where estado_desc = 'Finalizada')";

            int cantidad  = (int)QueryBuilder.Instance.build(cantidadCostos, parametros).ExecuteScalar();

            labelCantidadCostos.Text = cantidad.ToString();
        }

        private void CargarComisionesVentasPorFacturar()
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));

            String cantidadMinimaComisiones = "select COUNT(c.comp_id) from NET_A_CERO.Usuarios u,"
             + " NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p"
             + " where u.usr_id = @id and p.publi_usr_id = u.usr_id and c.comp_publi_id = p.publi_id"
             + " and p.publi_estado_id = (select estado_id from NET_A_CERO.Estado where estado_desc = 'Finalizada')";

            cantidadMin = (int)QueryBuilder.Instance.build(cantidadMinimaComisiones, parametros).ExecuteScalar();

            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));

            String cantidadMaximaComisiones = "select COUNT(c.comp_id) from NET_A_CERO.Usuarios u,"
             + " NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p"
             + " where u.usr_id = @id and p.publi_usr_id = u.usr_id and c.comp_publi_id = p.publi_id";

            cantidadMax = (int)QueryBuilder.Instance.build(cantidadMaximaComisiones, parametros).ExecuteScalar();

            dropDownFacturar.Text = cantidadMin.ToString();
            labelMinimo.Text = cantidadMin.ToString();
            labelMaximo.Text = cantidadMax.ToString();
            while (cantidadMax >= cantidadMin)
            {
                dropDownFacturar.Items.Add(cantidadMax);
                cantidadMax--;
            }
        }

        private void botonFacturar_Click(object sender, EventArgs e)
        {
            if (radioButtonTarjeta.Checked)
            {
                if (textBoxNumero.Text == "" || textBoxBanco.Text == "")
                {
                    MessageBox.Show("Para pagar con tarjeta de credito debe completar el numeroCalle de tarjeta y el Banco");
                    return;
                }

                long number1 = 0;
                if (!long.TryParse(textBoxNumero.Text, out number1))
                {
                    MessageBox.Show("El campo numeroCalle de tarjeta solo permite numeros");
                    return;
                }
            }

            if (labelCantidadCostos.Text != "0" || dropDownFacturar.Text != "0")
            {
            // Creo la nueva factura
                String creoFactura = "insert NET_A_CERO.Facturas"
                                + "(fecha) values(@fecha)";
            parametros.Clear();
            //parametros.Add(new SqlParameter("@fecha", Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["DateKey"]))); TP ANTERIOR
            parametros.Add(new SqlParameter("@fecha", DateConfig.getInstance().getCurrentDate()));
            QueryBuilder.Instance.build(creoFactura, parametros).ExecuteNonQuery();

            // Obtengo el id de la nueva factura
            String idFactura = "select top 1 f.fact_id from NET_A_CERO.Facturas f order by f.fact_ida DESC";
            parametros.Clear();
            Decimal idFact = (Decimal)QueryBuilder.Instance.build(idFactura,parametros).ExecuteScalar();

            // Inserto los items factura de costos por publicacion 
            String consulta = "NET_A_CERO.items";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));
            parametros.Add(new SqlParameter("@idF", idFact));
            SqlParameter parametroContador = new SqlParameter("@contador_bonificaciones", SqlDbType.Int);
            parametroContador.Direction = ParameterDirection.Output;
            parametros.Add(parametroContador);
            SqlParameter parametroMonto = new SqlParameter("@monto_descontado", SqlDbType.Decimal);
            parametroMonto.Direction = ParameterDirection.Output;
            parametros.Add(parametroMonto);
            command = QueryBuilder.Instance.build(consulta, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            int cantidadDeBonificaciones = (int)parametroContador.Value;
            Decimal montoDescontadoBonificaciones = 0;
            montoDescontadoBonificaciones = (Decimal)parametroMonto.Value;

            String costoPagado = "declare @pid numeric(18,0)"
                            + " declare publ_cursor cursor for"
                            + " (select p.publi_id from NET_A_CERO.Publicaciones p,"
                            + " NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u"
                            + " where p.publi_costo_pagado = 0 and p.publi_visib_id = v.visib_id and p.publi_usr_id = u.usr_id"
                            + " and u.usr_id = @id and p.publi_estado_id = (select estado_id from NET_A_CERO.Estado where estado_desc = 'Finalizada'))"
                            + " open publ_cursor"
                            + " fetch next from publ_cursor into @pid"
                            + " while @@FETCH_STATUS = 0"
                            + " Begin "
                            + " update NET_A_CERO.Publicaciones"
                            + " set costo_pagado = 1"
                            + " where id = @pid"
                            + " fetch next from publ_cursor into @pid"
                            + " End"
                            + " close publ_cursor"
                            + " deallocate publ_cursor";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));
            QueryBuilder.Instance.build(costoPagado, parametros).ExecuteNonQuery();

            int valor;
            Int32.TryParse(dropDownFacturar.Text, out valor);

            // Borro tabla temporal con ventas que se facturan pagando comision
            String borroTabla = "IF OBJECT_ID('NET_A_CERO.Facturas_temporarias', 'U') IS NOT NULL drop table NET_A_CERO.Facturas_temporarias";
            parametros.Clear();
            QueryBuilder.Instance.build(borroTabla, parametros).ExecuteNonQuery();

            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));
            parametros.Add(new SqlParameter("@cant", valor));

            // Creo una tabla con todas las ventas por facturar
            String totalidadVentasFacturar = "create table NET_A_CERO.Facturas_temporarias"
                                    + " (compra_id numeric(18,0),"
                                    + " compra_fecha datetime,"
                                    + " compra_publicacion numeric(18,0),"
                                    + " compra_cantidad numeric(18,0))"
                                    + " insert into NET_A_CERO.Facturas_temporarias"
                                    + " (compra_id, compra_fecha, compra_publicacion, compra_cantidad)" 
                                    + " select top (@cant) c.comp_id, c.comp_fecha, c.comp_publi_id, c.comp_cantidad"
                                    + " from NET_A_CERO.Usuarios u, NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p"
                                    + " where u.usr_id = @id and p.publi_usr_id = u.usr_id and c.comp_publi_id = p.publi_id"
                                    + " order by c.comp_fecha";
            QueryBuilder.Instance.build(totalidadVentasFacturar, parametros).ExecuteNonQuery();

            // Inserto los items factura de ventas
            String consulta2 = "NET_A_CERO.Items";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", UsuarioSesion.Usuario.id));
            parametros.Add(new SqlParameter("@idF", idFact));
            command = QueryBuilder.Instance.build(consulta2, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            // Actualizo el campo facturada en las ventas que facturo
            String ventaFacturada = "declare @cid numeric(18,0)"
                                + " declare compra_cursor cursor for"
                                + " (select c.comp_id from NET_A_CERO.Facturas_temporarias cc,"
                                + " NET_A_CERO.Compras c, NET_A_CERO.Publicaciones p,"
                                + " NET_A_CERO.Visibilidad v"
                                + " where cc.compra_id = c.comp_id and c.comp_publi_id = p.publi_id and p.publi_visib_id = v.id)"
                                + " open compra_cursor"
                                + " fetch next from compra_cursor into @cid"
                                + " while @@FETCH_STATUS = 0"
                                + " Begin"
                                + " update NET_A_CERO.Compras"
                               // + " set facturada = 1"
                                + " where id = @cid"
                                + " fetch next from compra_cursor into @cid"
                                + " End"
                                + " close compra_cursor"
                                + " deallocate compra_cursor";
            parametros.Clear();
            QueryBuilder.Instance.build(ventaFacturada,parametros).ExecuteNonQuery();

            // Inserto el total en la factura
            String actualizoTotal = "update NET_A_CERO.Facturas"
                               + " set total = (select SUM(i.monto)"
                               + " from NET_A_CERO.Items i"
                               + " where i.item_fact_id = nro) where nro = @idF";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idF", idFact));
            QueryBuilder.Instance.build(actualizoTotal, parametros).ExecuteNonQuery();

            // Inserto la forma de pago en la factura
            String formaPago = "update NET_A_CERO.Facturas"
                        + " set forma_pago_id = @pago";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idF", idFact));
            parametros.Add(new SqlParameter("@pago", formaDePago));
            QueryBuilder.Instance.build(formaPago, parametros).ExecuteNonQuery();

            // Borro tabla temporal con ventas que se facturan pagando comision
            String borroTablaTemporal = "IF OBJECT_ID('NET_A_CERO.Facturas_temporarias', 'U') IS NOT NULL drop table NET_A_CERO.Facturas_temporarias";
            parametros.Clear();
            QueryBuilder.Instance.build(borroTablaTemporal, parametros).ExecuteNonQuery();

            
                MessageBox.Show("Factura realizada. Por bonificaciones se desconto: $" + montoDescontadoBonificaciones);
                dropDownFacturar.Items.Clear();
            }
            else
            {
                MessageBox.Show("No hay nada para facturar");
            }
            CargarCostosPublicacionPorFacturar();
            CargarComisionesVentasPorFacturar();
            CalcularMonto();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void radioButtonTarjeta_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTarjeta.Checked)
            {
                textBoxNumero.Enabled = true;
                textBoxBanco.Enabled = true;
                formaDePago = "Tarjeta de credito";
            }
            else
            {
                textBoxNumero.Enabled = false;
                textBoxBanco.Enabled = false;
            }
        }

        private void radioButtonEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEfectivo.Checked)
            {
                formaDePago = "Efectivo";
            }
        }

        private void dropDownFacturar_SelectedItemChanged(object sender, EventArgs e)
        {
            CalcularMonto();
        }

        private void textBoxNumero_TextChanged(object sender, EventArgs e)
        {

        }

    }
}