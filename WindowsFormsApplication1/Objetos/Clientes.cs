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
        private Decimal id;
        private String nombre;
        private String apellido;
        private String numeroDeDocumento; 
        private String tipoDeDocumento;
        private DateTime fechaDeNacimiento;
        private DateTime fechaDeAlta;
        private Decimal idUsuario;

        public void SetId(Decimal id)
        {
            this.id = id;
        }

        public Decimal GetId()
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
            return "NET_A_CERO.crear_cliente";
        }

        string Comunicable.GetQueryModificar()
        {
            return "UPDATE NET_A_CERO.Clientes SET nombre = @nombre, apellido = @apellido, documento = @documento, tipo_de_documento = @tipo_de_documento, fecha_nacimiento = @fecha_nacimiento, fecha_Alta = @fecha_Alta WHERE id = @id";
        }

        string Comunicable.GetQueryObtener()
        {
            return "SELECT * FROM NET_A_CERO.Clientes WHERE id = @id";
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
            return parametros;
        }

        void Comunicable.CargarInformacion(SqlDataReader reader)
        {
            this.nombre = Convert.ToString(reader["nombre"]);
            this.apellido = Convert.ToString(reader["apellido"]);
            this.numeroDeDocumento = Convert.ToString(reader["documento"]);
            this.tipoDeDocumento = Convert.ToString(reader["tipo_de_documento"]);
            this.fechaDeNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]);
            this.idUsuario = Convert.ToDecimal(reader["usuario_id"]);
        }

        #endregion
    }
}