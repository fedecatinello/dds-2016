using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using System.Data.SqlClient;

namespace MercadoEnvio.Modelo
{
    class Publicacion : Objeto, Mapeable
    {
        private Decimal id;
        private String tipoDePublicacion;
        private Decimal idEstado;
        private String descripcion;
        private DateTime fechaDeInicio;
        private DateTime fechaDeVencimiento;
        private Decimal idRubro;
        private Decimal idVisibilidad;
        private Decimal idUsuario;
        private String stock;
        private String precio;
        private Boolean pregunta;
        private Boolean habilitado;
        private Boolean costoPagado;

        public void SetId(Decimal id)
        {
            this.id = id;
        }

        public Decimal GetId()
        {
            return this.id;
        }

        public void SetCostoPagado(Boolean costoPagado)
        {
            this.costoPagado = costoPagado;
        }

        public Boolean GetCostoPagado()
        {
            return this.costoPagado;
        }

        public void SetTipo(String tipoDePublicacion)
        {
            this.tipoDePublicacion = tipoDePublicacion;
        }

        public String GetTipo()
        {
            return this.tipoDePublicacion;
        }

        public void SetEstado(Decimal idEstado)
        {
            this.idEstado = idEstado;
        }

        public Decimal GetEstado()
        {
            return this.idEstado;
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

        public void SetFechaDeInicio(DateTime fechaDeInicio)
        {
            this.fechaDeInicio = fechaDeInicio;
        }

        public DateTime GetFechaDeInicio()
        {
            return this.fechaDeInicio;
        }

        public void SetFechaDeVencimiento(DateTime fechaDeVencimiento)
        {
            this.fechaDeVencimiento = fechaDeVencimiento;
        }

        public DateTime GetFechaDeVencimiento()
        {
            return this.fechaDeVencimiento;
        }

        public void SetIdRubro(Decimal idRubro)
        {
            if (idRubro == 0)
                throw new CampoVacioException("Rubro");
            this.idRubro = idRubro;
        }

        public Decimal GetIdRubro()
        {
            return this.idRubro;
        }

        public void SetIdUsuario(Decimal idUsuario)
        {
            if (idUsuario == 0)
                throw new CampoVacioException("Usuario");
            this.idUsuario = idUsuario;
        }

        public Decimal GetIdUsuario()
        {
            return this.idUsuario;
        }

        public void SetIdVisibilidad(Decimal idVisibilidad)
        {
            if (idVisibilidad == 0)
                throw new CampoVacioException("Visibilidad");
            this.idVisibilidad = idVisibilidad;
        }

        public Decimal GetIdVisibilidad()
        {
            return this.idVisibilidad;
        }

        public void SetStock(String stock)
        {
            if (stock == "")
                throw new CampoVacioException("Stock");

            if (!esNumero(stock))
                throw new FormatoInvalidoException("Stock");

            this.stock = stock;
        }

        public String GetStock()
        {
            return this.stock;
        }

        public void SetPrecio(String precio)
        {
            if (precio == "")
                throw new CampoVacioException("Precio");

            if (!esNumero(precio))
                throw new IngresePrecioEnteroException("Precio");

            this.precio = precio;
        }

        public String GetPrecio()
        {
            return this.precio;
        }

        public void SetPregunta(Boolean pregunta)
        {
            this.pregunta = pregunta;
        }

        public Boolean GetPregunta()
        {
            return this.pregunta;
        }

        public void SetHabilitado(Boolean habilitado)
        {
            this.habilitado = habilitado;
        }

        public Boolean GetHabilitado()
        {
            return this.habilitado;
        }


        #region Miembros de Comunicable

        string Mapeable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_publicacion";
        }

        string Mapeable.GetQueryModificar()
        {
            return " UPDATE NET_A_CERO.Publicaciones SET publi_tipo = @publi_tipo, publi_descripcion = @publi_descripcion, publi_stock = @publi_stock, publi_fec_vencimiento = @publi_fec_vencimiento, publi_fec_inicio = @publi_fec_inicio, publi_precio = @publi_precio, publi_costo_pagado = @publi_costo_pagado, publi_preguntas = @publi_preguntas, publi_visib_id = @publi_visib_id, publi_estado_id = @publi_estado_id WHERE publi_id = @id " +
                " DELETE NET_A_CERO.Rubro_x_Publicacion WHERE publi_id = @id " +    
                " INSERT INTO NET_A_CERO.Rubro_x_Publicacion (rubro_id, publi_id) VALUES (@rubro_id, @id) ";
        }

        string Mapeable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Publicaciones WHERE publi_id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Mapeable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@publi_tipo", this.tipoDePublicacion));
            parametros.Add(new SqlParameter("@publi_descripcion", this.descripcion));
            parametros.Add(new SqlParameter("@publi_stock", this.stock));
            parametros.Add(new SqlParameter("@publi_fec_vencimiento", this.fechaDeVencimiento));
            parametros.Add(new SqlParameter("@publi_fec_inicio", this.fechaDeInicio));
            parametros.Add(new SqlParameter("@publi_precio", this.precio));
            parametros.Add(new SqlParameter("@publi_costo_pagado", this.costoPagado));
            parametros.Add(new SqlParameter("@publi_preguntas", this.pregunta));
            parametros.Add(new SqlParameter("@publi_usr_id", this.idUsuario));
            parametros.Add(new SqlParameter("@publi_visib_id", this.idVisibilidad));
            parametros.Add(new SqlParameter("@publi_estado_id", this.idEstado));
            parametros.Add(new SqlParameter("@rubro_id", this.idRubro));
            //parametros.Add(new SqlParameter("@publi_id", this.id));
            return parametros;
        }

        void Mapeable.CargarInformacion(SqlDataReader reader)
        {
            this.tipoDePublicacion = Convert.ToString(reader["publi_tipo"]);
            this.descripcion = Convert.ToString(reader["publi_descripcion"]);
            this.stock = Convert.ToString(reader["publi_stock"]);
            this.fechaDeVencimiento = Convert.ToDateTime(reader["publi_fec_vencimiento"]);
            this.fechaDeInicio = Convert.ToDateTime(reader["publi_fec_inicio"]);
            this.precio = Convert.ToString(reader["publi_precio"]);
            this.costoPagado = Convert.ToBoolean(reader["publi_costo_pagado"]);
            this.pregunta = Convert.ToBoolean(reader["publi_preguntas"]);
            this.idUsuario = Convert.ToDecimal(reader["publi_usr_id"]);
            this.idVisibilidad = Convert.ToDecimal(reader["publi_visib_id"]);
            this.idEstado = Convert.ToDecimal(reader["publi_estado_id"]);
            //this.idRubro = Convert.ToDecimal(reader["publi_rubro_id"]);
        }

        #endregion
    }
}