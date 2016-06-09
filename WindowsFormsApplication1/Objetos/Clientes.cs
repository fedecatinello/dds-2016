using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercadoEnvio.Exceptions;
using System.Data.SqlClient;

namespace MercadoEnvio.Objetos
{
    class Clientes : Objeto, Comunicable
    {
        private int id;
        private String nombre;
        private String apellido;
        private String numeroDeDocumento; 
        private String tipoDeDocumento;
        private DateTime fechaDeNacimiento;
        private DateTime fechaDeAlta;
        private int idUsuario;
        private int idContacto;

        public void SetId(int id)
        {
            this.id = id;
        }

        public int GetId()
        {
            return this.id;
        }

        public void SetNombre(String nombre)
        {
            if (nombre == "")
                throw new CampoVacioException("Nombre");
            this.nombre = nombre;
        }

        public String GetNombre()
        {
            return this.nombre;
        }

        public void SetApellido(String apellido)
        {
            if (apellido == "")
                throw new CampoVacioException("Apellido");
            this.apellido = apellido;
        }

        public String GetApellido()
        {
            return this.apellido;
        }

        public void SetTipoDeDocumento(String tipoDeDocumento)
        {
            this.tipoDeDocumento = tipoDeDocumento;
        }

        public String GetTipoDeDocumento()
        {
            return this.tipoDeDocumento;
        }

        public void SetNumeroDeDocumento(String numeroDeDocumento)
        {
            if (numeroDeDocumento == "")
                throw new CampoVacioException("Numero de documento");

            if (!esNumero(numeroDeDocumento))
                throw new FormatoInvalidoException("Numero de documento. Ingrese todos los numeros seguidos.");

            this.numeroDeDocumento = numeroDeDocumento;
        }

        public String GetNumeroDeDocumento()
        {
            return this.numeroDeDocumento;
        }

        public void SetFechaDeNacimiento(DateTime fechaDeNacimiento)
        {
            if (fechaDeNacimiento.Equals(DateTime.MinValue))
                throw new CampoVacioException("Fecha de nacimiento");

            if (!esFechaPasada(fechaDeNacimiento))
                throw new FechaPasadaException();

            this.fechaDeNacimiento = fechaDeNacimiento;
        }

        public DateTime GetFechaDeNacimiento()
        {
            return this.fechaDeNacimiento;
        }

        public void SetFechaDeAlta (DateTime fechaDeAlta)
        {
            this.fechaDeAlta = fechaDeAlta;
        }

        public DateTime GetFechaDeAlta()
        {
            return this.fechaDeAlta;
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


        #region Miembros de Comunicable

        string Comunicable.GetQueryCrear()
        {
            return "NET_A_CERO.pr_crear_cliente";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Clientes SET cli_nombre = @nombre, cli_apellido = @apellido, cli_documento = @documento, cli_tipo_dni = @tipo_de_documento, cli_fecha_nac = @fecha_nacimiento, cli_fecha_alta = @fecha_alta WHERE cli_id = @id";
        }

        string Comunicable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Clientes WHERE cli_id = @id";
        }

        IList<System.Data.SqlClient.SqlParameter> Comunicable.GetParametros()
        {
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Clear();
            parametros.Add(new SqlParameter("@nombre", this.nombre));
            parametros.Add(new SqlParameter("@apellido", this.apellido));
            parametros.Add(new SqlParameter("@documento", this.numeroDeDocumento));
            parametros.Add(new SqlParameter("@tipo_de_documento", this.tipoDeDocumento));
            parametros.Add(new SqlParameter("@fecha_nacimiento", this.fechaDeNacimiento));
            parametros.Add(new SqlParameter("@fecha_alta", this.fechaDeAlta));
            parametros.Add(new SqlParameter("@cont_id", this.idContacto));
            return parametros;
        }

        void Comunicable.CargarInformacion(SqlDataReader reader)
        {
            this.nombre = Convert.ToString(reader["cli_nombre"]);
            this.apellido = Convert.ToString(reader["cli_apellido"]);
            this.numeroDeDocumento = Convert.ToString(reader["cli_dni"]);
            this.tipoDeDocumento = Convert.ToString(reader["cli_tipo_dni"]);
            this.fechaDeNacimiento = Convert.ToDateTime(reader["cli_fecha_nac"]);
            this.fechaDeAlta = Convert.ToDateTime(reader["cli_fecha_alta"]);
            this.idUsuario = Convert.ToInt32(reader["cli_usr_id"]);
            this.idContacto = Convert.ToInt32(reader["cli_cont_id"]);
        }

        #endregion
    }
}