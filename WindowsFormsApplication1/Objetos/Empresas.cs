using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using MercadoEnvio.DataProvider;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MercadoEnvio.Objetos
{
    class Empresas : Objeto, Comunicable
    {
        private int id;
        private String razonSocial;
        private String ciudad;
        private String cuit;
        private String nombreDeContacto;
        private int rubro;
        private DateTime fechaDeCreacion;
        private int idUsuario;
        private int idContacto;

        private IList<SqlParameter> parametros = new List<SqlParameter>();
        

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
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@rubro", this.rubro));
            string queryRubro = "SELECT rubro_desc_larga FROM NET_A_CERO.Rubros WHERE rubro_id = @rubro";

            SqlDataReader reader = QueryHelper.Instance.exec(queryRubro, parametros);
            return (string)reader["rubro_desc_larga"];
        }

        public void SetRubroEmpresa(string rubro)
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@rubro", rubro));
            string queryRubro = "SELECT rubro_id FROM NET_A_CERO.Rubros WHERE rubro_desc_larga = @rubro";

            SqlDataReader reader = QueryHelper.Instance.exec(queryRubro, parametros);
            this.rubro = (int)reader["rubro_id"];

            //if(this.rubro == null) 
        }


        #region Miembros de Comunicable

        string Comunicable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_empresa";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Empresas SET emp_razon_social = @razon_social, emp_ciudad = @ciudad, emp_cuit = @cuit, emp_nombre_contacto = @nombre_contacto, emp_rubro = (SELECT rubro_id FROM NET_A_CERO.Rubros WHERE rubro_desc_larga = @rubro), emp_fecha_alta = @fecha_alta WHERE emp_id = @id";
        }

        public string GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Empresas WHERE emp_id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Comunicable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@razon_social", this.razonSocial));
            parametros.Add(new SqlParameter("@ciudad", this.ciudad));
            parametros.Add(new SqlParameter("@cuit", this.cuit));
            parametros.Add(new SqlParameter("@nombre_contacto", this.nombreDeContacto));
            parametros.Add(new SqlParameter("@rubro", this.rubro));
            parametros.Add(new SqlParameter("@fecha_alta", this.fechaDeCreacion));
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
            this.idUsuario = Convert.ToInt32(reader["emp_usr_id"]);
            this.idContacto = Convert.ToInt32(reader["emp_cont_id"]);
        }

        #endregion
    }
}