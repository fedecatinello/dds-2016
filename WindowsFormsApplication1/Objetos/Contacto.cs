using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MercadoEnvio.Exceptions;
using MercadoEnvio.Objetos;

namespace MercadoEnvio
{
    class Contacto : Objeto, Comunicable
    {
        private int id;
        private String mail;
        private String telefono;
        private String calle;
        private String numeroCalle;
        private String piso;
        private String departamento;
        private String localidad;
        private String codigoPostal;
      


        public void setMail(String mail)
        {
            if (mail == "")
                throw new CampoVacioException("Mail");
            this.mail = mail;
        }
        
        public void setTelefono(String telefono)
        {
            if (telefono == "")
                throw new CampoVacioException("Telefono");
            this.telefono = telefono;
        }

        public void SetCalle(String calle)
        {
            if (calle == "")
                throw new CampoVacioException("Calle");
            this.calle = calle;
        }

        public void SetNumeroCalle(String numeroCalle)
        {
            if (numeroCalle == "")
                throw new CampoVacioException("Numero");

            if (!esNumero(numeroCalle))
                throw new FormatoInvalidoException("Numero");

            this.numeroCalle = numeroCalle;
        }

        public void SetPiso(String piso)
        {
            if (piso != "" && !esNumero(piso))
                throw new FormatoInvalidoException("Piso");

            this.piso = piso;
        }

        public void SetDepartamento(String departamento)
        {
            this.departamento = departamento;
        }

        public void SetCodigoPostal(String codigoPostal)
        {
            if (codigoPostal == "")
                throw new CampoVacioException("Codigo postal");

            if (!esNumero(codigoPostal))
                throw new FormatoInvalidoException("Codigo postal");

            this.codigoPostal = codigoPostal;
        }

        public void SetLocalidad(String localidad)
        {
            if (localidad == "")
                throw new CampoVacioException("Localidad");
            this.localidad = localidad;
        }

        public int GetId()
        {
            return this.id;
        }
        public String GetMail()
        {
            return this.mail;
        }

        public String GetTelefono()
        {
            return this.telefono;
        }

        public String GetCalle()
        {
            return this.calle;
        }

        public String GetNumeroCalle()
        {
            return this.numeroCalle;
        }

        public String GetPiso()
        {
            return this.piso;
        }

        public String GetDepartamento()
        {
            return this.departamento;
        }

        public String GetCodigoPostal()
        {
            return this.codigoPostal;
        }

        public String GetLocalidad()
        {
            return this.localidad;
        }
      
     

        #region Miembros de Comunicable

        string Comunicable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_contacto";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Contacto SET cont_mail = @mail, cont_telefono = @telefono, cont_calle = @calle, cont_numero_calle = @numeroCalle, cont_piso = @piso, cont_depto = @depto, cont_localidad = @localidad, cont_codigo_postal = @cod_postal WHERE cont_id = @id";
        }

        string Comunicable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Contacto WHERE cont_id = @id";
        }

        IList<SqlParameter> Comunicable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@mail", this.mail));
            parametros.Add(new SqlParameter("@telefono", this.telefono));
            parametros.Add(new SqlParameter("@calle", this.calle));
            parametros.Add(new SqlParameter("@numeroCalle", this.numeroCalle));
            parametros.Add(new SqlParameter("@piso", this.siEsNuloDevolverDBNull(piso)));
            parametros.Add(new SqlParameter("@depto", this.siEsNuloDevolverDBNull(departamento)));
            parametros.Add(new SqlParameter("@cod_postal", this.codigoPostal));
            parametros.Add(new SqlParameter("@localidad", this.localidad));
            return parametros;
        }

        void Comunicable.CargarInformacion(SqlDataReader reader)
        {
            this.mail = Convert.ToString(reader["cont_mail"]);
            this.telefono = Convert.ToString(reader["cont_telefono"]);
            this.calle = Convert.ToString(reader["cont_calle"]);
            this.numeroCalle = Convert.ToString(reader["cont_numero_calle"]);
            this.piso = Convert.ToString(reader["cont_piso"]);
            this.departamento = Convert.ToString(reader["cont_depto"]);
            this.codigoPostal = Convert.ToString(reader["cont_codigo_postal"]);
            this.localidad = Convert.ToString(reader["cont_localidad"]);
        }

        #endregion
    }
}