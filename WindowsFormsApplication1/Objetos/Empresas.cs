using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using System.Data.SqlClient;

namespace MercadoEnvio.Objetos
{
    class Empresas : Objeto, Comunicable
    {
        private Decimal id;
        private String razonSocial;
        private String ciudad;
        private String cuit;
        private String nombreDeContacto;
        private String rubro;
        private DateTime fechaDeCreacion;
        private Decimal idUsuario;
        

        public void SetId(Decimal id)
        {
            this.id = id;
        }

        public Decimal GetId()
        {
            return this.id;
        }

        public void SetRazonSocial(String razonSocial)
        {
            if (razonSocial == "")
                throw new CampoVacioException("Razon social");
            this.razonSocial = razonSocial;
        }

        public String GetRazonSocial()
        {
            return this.razonSocial;
        }

        public void SetCiudad(String ciudad)
        {
            if (ciudad == "")
                throw new CampoVacioException("Ciudad");
            this.ciudad = ciudad;
        }

        public String GetCiudad()
        {
            return this.ciudad;
        }

        public void SetCuit(String cuit)
        {
            if (cuit == "")
                throw new CampoVacioException("CUIT");

            if (!esCuit(cuit))
                throw new FormatoInvalidoException("CUIT. Usar el siguiente formato: XX-XXXXXXXX-XX donde X es numeroCalle");
            this.cuit = cuit;
        }

        public String GetCuit()
        {
            return this.cuit;
        }

        public void SetNombreDeContacto(String nombreDeContacto)
        {
            if (nombreDeContacto == "")
                throw new CampoVacioException("Nombre de contacto");
            this.nombreDeContacto = nombreDeContacto;
        }

        public String GetNombreDeContacto()
        {
            return this.nombreDeContacto;
        }

        public void SetRubro(String rubro)
        {
            if (rubro == "")
                throw new CampoVacioException("Rubro");
            this.rubro = rubro;
        }

        public string GetRubro()
        {
            return this.rubro;
        }


        public void SetFechaDeCreacion(DateTime fechaDeCreacion)
        {
            if (fechaDeCreacion.Equals(DateTime.MinValue))
                throw new CampoVacioException("Fecha de creacion");

            if (!esFechaPasada(fechaDeCreacion))
                throw new FechaPasadaException();

            this.fechaDeCreacion = fechaDeCreacion;
        }

        public DateTime GetFechaDeCreacion()
        {
            return this.fechaDeCreacion;
        }

        public void SetIdUsuario(Decimal idUsuario)
        {
            this.idUsuario = idUsuario;
        }

        public Decimal GetIdUsuario()
        {
            return this.idUsuario;
        }


        #region Miembros de Comunicable

        string Comunicable.GetQueryCrear()
        {
            return "NET_A_CERO.crear_empresa";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Empresas SET razon_social = @razon_social, ciudad = @ciudad, cuit = @cuit, nombre_de_contacto = @nombre_de_contacto, rubro = @rubro, fecha_creacion = @fecha_creacion WHERE id = @id";
        }

        public string GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Empresa WHERE id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Comunicable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@razon_social", this.razonSocial));
            parametros.Add(new SqlParameter("@ciudad", this.ciudad));
            parametros.Add(new SqlParameter("@cuit", this.cuit));
            parametros.Add(new SqlParameter("@nombre_de_contacto", this.nombreDeContacto));
            parametros.Add(new SqlParameter("@rubro", this.rubro));
            parametros.Add(new SqlParameter("@fecha_creacion", this.fechaDeCreacion));
            return parametros;
        }

        public void CargarInformacion(SqlDataReader reader)
        {
            this.razonSocial = Convert.ToString(reader["razon_social"]);
            this.ciudad = Convert.ToString(reader["ciudad"]);
            this.cuit = Convert.ToString(reader["cuit"]);
            this.nombreDeContacto = Convert.ToString(reader["nombre_de_contacto"]);
            this.rubro = Convert.ToString(reader["rubro"]);
            this.fechaDeCreacion = Convert.ToDateTime(reader["fecha_creacion"]);
            this.idUsuario = Convert.ToDecimal(reader["usuario_id"]);
        }

        #endregion
    }
}