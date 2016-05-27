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
        private int usr_id;


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

        public void SetNumero(String numeroCalle)
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

        public void SetId(int id)
        {
            this.id = id;
        }

        public String GetCalle()
        {
            return this.calle;
        }

        public String GetNumero()
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

        public Decimal GetId()
        {
            return this.id;
        }

        #region Miembros de Comunicable

        string Comunicable.GetQueryCrear()
        {
            return "NET_A_CERO.crear_contacto";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Contacto SET mail=@mail, telefono=@telefono, calle = @calle, numeroCalle = @numeroCalle, piso = @piso, depto = @depto, cod_postal = @cod_postal, localidad = @localidad WHERE id = @id";
        }

        string Comunicable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Contacto WHERE id = @id";
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
            this.mail = Convert.ToString(reader["mail"]);
            this.telefono = Convert.ToString(reader["telefono"]);
            this.calle = Convert.ToString(reader["calle"]);
            this.numeroCalle = Convert.ToString(reader["numeroCalle"]);
            this.piso = Convert.ToString(reader["piso"]);
            this.departamento = Convert.ToString(reader["depto"]);
            this.codigoPostal = Convert.ToString(reader["cod_postal"]);
            this.localidad = Convert.ToString(reader["localidad"]);
        }

        #endregion
    }
}