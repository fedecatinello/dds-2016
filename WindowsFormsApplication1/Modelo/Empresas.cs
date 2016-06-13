using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using MercadoEnvio.DataProvider;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MercadoEnvio.Modelo
{
    class Empresas : Objeto, Mapeable
    {
        private int id;
        private String razonSocial;
        private String ciudad;
        private String cuit;
        private String nombreDeContacto;
        private int rubro;
        private DateTime fechaDeCreacion;
        private Boolean activo;
        private int idUsuario;
        private int idContacto;

        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private DBMapper mapper = new DBMapper();
        

        public void SetId(int id)
        {
            this.id = id;
        }

        public int GetId()
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
            SetRubroEmpresa(rubro);
        }

        public string GetRubro()
        {
            return GetDescripcionRubroEmpresa();
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

        public void SetActivo(Boolean emp_activo)
        {
            this.activo = emp_activo;
        }

        public Boolean GetActivo()
        {
            return this.activo;
        }

        public void SetIdUsuario(int idUsuario)
        {
            this.idUsuario = idUsuario;
        }

        public int GetIdUsuario()
        {
            return this.idUsuario;
        }

        public void SetIdContacto(int idContacto)
        {
            this.idContacto = idContacto;
        }

        public int GetIdContacto()
        {
            return this.idContacto;
        }

        public string GetDescripcionRubroEmpresa()
        {
            return Convert.ToString(mapper.SelectFromWhere("rubro_desc_larga", "Rubros", "rubro_id", this.rubro));
        }

        public void SetRubroEmpresa(string rubro)
        {
            this.rubro = Convert.ToInt32(mapper.SelectFromWhere("rubro_id", "Rubros", "rubro_desc_larga", rubro));
        }


        #region Miembros de Comunicable

        string Mapeable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_empresa";
        }

        string Mapeable.GetQueryModificar()
        {
            if (activo == true)
            {
                return "UPDATE NET_A_CERO.Empresas SET emp_razon_social = @razon_social, emp_ciudad = @ciudad, emp_cuit = @cuit, emp_nombre_contacto = @nombre_contacto, emp_rubro = @rubro, emp_fecha_alta = @fecha_alta, emp_activo = @activo WHERE emp_id = @id" +
                    " UPDATE NET_A_CERO.Usuarios SET usr_intentos = 0 WHERE usr_id = (SELECT emp_usr_id FROM NET_A_CERO.Empresas WHERE emp_id = @id) ";
            }
            else
            {
                return "UPDATE NET_A_CERO.Empresas SET emp_razon_social = @razon_social, emp_ciudad = @ciudad, emp_cuit = @cuit, emp_nombre_contacto = @nombre_contacto, emp_rubro = @rubro, emp_fecha_alta = @fecha_alta, emp_activo = @activo WHERE emp_id = @id";
            }

        }

        public string GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Empresas WHERE emp_id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Mapeable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@razon_social", this.razonSocial));
            parametros.Add(new SqlParameter("@ciudad", this.ciudad));
            parametros.Add(new SqlParameter("@cuit", this.cuit));
            parametros.Add(new SqlParameter("@nombre_contacto", this.nombreDeContacto));
            parametros.Add(new SqlParameter("@rubro", this.rubro));
            parametros.Add(new SqlParameter("@fecha_alta", this.fechaDeCreacion));
            parametros.Add(new SqlParameter("@activo", true));
            parametros.Add(new SqlParameter("@cont_id", this.idContacto));
            return parametros;
        }

        public void CargarInformacion(SqlDataReader reader)
        {
            this.razonSocial = Convert.ToString(reader["emp_razon_social"]);
            this.ciudad = Convert.ToString(reader["emp_ciudad"]);
            this.cuit = Convert.ToString(reader["emp_cuit"]);
            this.nombreDeContacto = Convert.ToString(reader["emp_nombre_contacto"]);
            this.rubro = Convert.ToInt32(reader["emp_rubro"]);
            this.fechaDeCreacion = Convert.ToDateTime(reader["emp_fecha_alta"]);
            this.activo = Convert.ToBoolean(reader["emp_activo"]);
            this.idUsuario = Convert.ToInt32(reader["emp_usr_id"]);
            this.idContacto = Convert.ToInt32(reader["emp_cont_id"]);
        }

        #endregion
    }
}