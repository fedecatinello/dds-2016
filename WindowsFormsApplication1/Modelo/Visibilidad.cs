using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using System.Data.SqlClient;
using MercadoEnvio.DataProvider;
using System.Windows.Forms;

namespace MercadoEnvio.Modelo
{
    class Visibilidad : Objeto, Mapeable
    {
        private Decimal id;
        private String descripcion;
        private String precioPorPublicar;
        private String porcentajePorVenta;
        private String grado;
        //private String duracion; No haria falta - Borrar si no hace falta
        private Boolean activo;
        private Boolean envios;

        public void SetId(Decimal id)
        {
            this.id = id;
        }

        public Decimal GetId()
        {
            return this.id;
        }

        public void SetDescripcion(String descripcion)
        {
            if (descripcion == "")
                throw new CampoVacioException("Descripcion");
            this.descripcion = descripcion;
        }

        public String GetDescripcion()
        {
            return this.descripcion;
        }

        public void SetPrecioPorPublicar(String precioPorPublicar)
        {
            if (precioPorPublicar == "")
                throw new CampoVacioException("Precio por publicar");

            if (!esDouble(precioPorPublicar))
                throw new FormatoInvalidoException("Precio por publicar");

            this.precioPorPublicar = precioPorPublicar;
        }

        public String GetPrecioPorPublicar()
        {
            return this.precioPorPublicar;
        }

        public void SetPorcentajePorVenta(String porcentajePorVenta)
        {
            if (porcentajePorVenta == "")
                throw new CampoVacioException("Porcentaje por venta");

            if (!esDouble(porcentajePorVenta))
                throw new FormatoInvalidoException("Porcentaje por venta");

            this.porcentajePorVenta = porcentajePorVenta;
        }

        public String GetPorcentajePorVenta()
        {
            return this.porcentajePorVenta;
        }

        /* public void SetDuracion(String duracion)
        {
            if (duracion == "")
                throw new CampoVacioException("Duracion");

            if (!esNumero(duracion))
                throw new FormatoInvalidoException("Duracion");

            this.duracion = duracion;
        }

        public String GetDuracion()
        {
            return this.duracion;
        }
         * */

        public void SetGrado(String grado_visib)
        {
            if (grado_visib == "")
                throw new CampoVacioException("Grado");
            this.grado = grado_visib;
        }

        public String GetGrado()
        {
            return this.grado;
        }

        public void SetActivo(Boolean visib_activo)
        {
            this.activo = visib_activo;
        }

        public Boolean GetActivo()
        {
            return this.activo;
        }

        public void SetEnvios(Boolean visib_envios)
        {
            this.envios = visib_envios;
        }

        public Boolean GetEnvios()
        {
            return this.envios;
        }

        public void CalcularIdAInsertar()
        {
            string query = "SELECT TOP 1 visib_id FROM NET_A_CERO.Visibilidad ORDER BY visib_id DESC";
            SetId(Convert.ToDecimal(QueryBuilder.Instance.build(query, null).ExecuteScalar()) + 1); //Inserto despues del ultimo registro  
        }

        public bool Persist()
        {
            string query = "NET_A_CERO.pr_crear_visibilidad";
            
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@id", GetId()));
            parametros.Add(new SqlParameter("@descripcion", this.descripcion));
            parametros.Add(new SqlParameter("@grado", this.grado));
            parametros.Add(new SqlParameter("@precio", Convert.ToDouble(this.precioPorPublicar)));
            parametros.Add(new SqlParameter("@porcentaje", Convert.ToDouble(this.porcentajePorVenta)));
            parametros.Add(new SqlParameter("@envios", this.envios));
            parametros.Add(new SqlParameter("@activo", this.activo));

            int filas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filas > 0) 
                return true;
            return false;
        }

        #region Miembros de Comunicable

        string Mapeable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_visibilidad";
        }

        string Mapeable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Visibilidad SET visib_desc = @descripcion, visib_grado = @grado, visib_precio = @precio, visib_porcentaje = @porcentaje, visib_envios = @envios, visib_activo = @activo WHERE visib_id = @id";
        }

        string Mapeable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Visibilidad WHERE visib_id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Mapeable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@descripcion", this.descripcion));
            parametros.Add(new SqlParameter("@grado", this.grado));
            parametros.Add(new SqlParameter("@precio", Convert.ToDouble(this.precioPorPublicar)));
            parametros.Add(new SqlParameter("@porcentaje", Convert.ToDouble(this.porcentajePorVenta)));
            parametros.Add(new SqlParameter("@envios", this.envios));
            parametros.Add(new SqlParameter("@activo", this.activo));
            //parametros.Add(new SqlParameter("@duracion", this.duracion));
            return parametros;
        }

        void Mapeable.CargarInformacion(SqlDataReader reader)
        {
            this.id = Convert.ToDecimal(reader["visib_id"]);
            this.descripcion = Convert.ToString(reader["visib_desc"]);
            this.grado = Convert.ToString(reader["visib_grado"]);
            this.precioPorPublicar = Convert.ToString(reader["visib_precio"]);
            this.porcentajePorVenta = Convert.ToString(reader["visib_porcentaje"]);
            this.envios = Convert.ToBoolean(reader["visib_envios"]);
            this.activo = Convert.ToBoolean(reader["visib_activo"]);
            //this.duracion =  Convert.ToString(reader["duracion"]);
        }

        #endregion
    }
}