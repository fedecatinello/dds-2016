using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MercadoEnvio.Objetos;
using MercadoEnvio.Exceptions;
using MercadoEnvio.DataProvider;
using MercadoEnvio.Utils;
using System.Data;

namespace MercadoEnvio
{
    class ComunicadorConBaseDeDatos
    {
        private String query;
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private SqlParameter parametroOutput;
        private SqlCommand command;
        
        public Decimal CrearUsuario()
        {
            query = "NET_A_CERO.crear_usuario";
            parametros.Clear();
            parametroOutput = new SqlParameter("@usuario_id", SqlDbType.Decimal);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (Decimal)parametroOutput.Value;
        }

        public Decimal CrearUsuarioConValores(String username, String password)
        {
            query = "NET_A_CERO.crear_usuario_con_valores";
            parametros.Clear();
            parametroOutput = new SqlParameter("@usuario_id", SqlDbType.Decimal);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(new SqlParameter("@username", username));
            parametros.Add(new SqlParameter("@password", HashSha256.getHash(password)));
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (Decimal)parametroOutput.Value;
        }

        public Boolean AsignarUsuarioACliente(Decimal idCliente, Decimal idUsuario)
        {
            query = "UPDATE NET_A_CERO.Clientes SET usr_id = @idUsuario WHERE cli_id = @idCliente";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idUsuario", idUsuario));
            parametros.Add(new SqlParameter("@idCliente", idCliente));
            command = QueryBuilder.Instance.build(query, parametros);
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }
        public Boolean AsignarUsuarioAEmpresa(Decimal idEmpresa, Decimal idUsuario)
        {
            query = "UPDATE NET_A_CERO.Empresas SET usr_id = @idUsuario WHERE emp_id = @idEmpresa";
            parametros.Clear();
            parametros.Add(new SqlParameter("@idUsuario", idUsuario));
            parametros.Add(new SqlParameter("@idEmpresa", idEmpresa));
            command = QueryBuilder.Instance.build(query, parametros);
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }


        public Boolean AsignarRolAUsuario(Decimal idUsuario, String rol)
        {
            Decimal idRol = Convert.ToDecimal(this.SelectFromWhere("id", "Rol", "nombre", rol));
            query = "NET_A_CERO.agregar_rol_a_usuario";
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario_id", idUsuario));
            parametros.Add(new SqlParameter("@rol_id", idRol));
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Decimal Crear(Comunicable objeto)
        {
            query = objeto.GetQueryCrear();
            parametros.Clear();
            parametros = objeto.GetParametros();
            parametroOutput = new SqlParameter("@id", SqlDbType.Decimal);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (Decimal)parametroOutput.Value;
        }

        public Boolean Modificar(Decimal id, Comunicable objeto)
        {
            query = objeto.GetQueryModificar();
            parametros.Clear();
            parametros = objeto.GetParametros();
            parametros.Add(new SqlParameter("@id", id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Comunicable Obtener(Decimal id, Type clase)
        {
            Comunicable objeto = (Comunicable)Activator.CreateInstance(clase);
            query = objeto.GetQueryObtener();
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", id));
            SqlDataReader reader = QueryBuilder.Instance.build(query, parametros).ExecuteReader();
            if (reader.Read())
            {
                objeto.CargarInformacion(reader);
                return objeto;
            }
            return objeto;
        }

        public Boolean Eliminar(Decimal id, String enDonde)
        {
            query = "UPDATE NET_A_CERO." + enDonde + " SET dado_de_baja = 1 WHERE id = @id";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Decimal CrearCliente(Cliente cliente)
        {
            if (!pasoControlDeRegistro(cliente.GetIdTipoDeDocumento(), cliente.GetNumeroDeDocumento()))
                throw new ClienteYaExisteException();

            if (!pasoControlDeUnicidad(cliente.GetTelefono(), "telefono", "Cliente"))
                throw new TelefonoYaExisteException();

            return this.Crear(cliente);
        }

        public Decimal CrearEmpresa(Empresa empresa)
        {
            if (!pasoControlDeRegistroDeCuit(empresa.GetCuit()))
                throw new CuitYaExisteException();

            if (!pasoControlDeUnicidad(empresa.GetTelefono(), "telefono", "Empresa"))
                throw new TelefonoYaExisteException();

            if (!pasoControlDeRegistroDeRazonSocial(empresa.GetRazonSocial()))
                throw new RazonSocialYaExisteException();

            return this.Crear(empresa);
        }

        public Decimal CrearDireccion(Direccion direccion)
        {
            return this.Crear(direccion);
        }

        public Decimal CrearPublicacion(Publicacion publicacion)
        {
            return this.Crear(publicacion);
        }

        public Decimal CrearVisibilidad(Visibilidad visibilidad)
        {
            if (!pasoControlDeUnicidad(visibilidad.GetDescripcion(), "descripcion", "Visibilidad"))
                throw new VisibilidadYaExisteException();

            return this.Crear(visibilidad);
        }

        public Cliente ObtenerCliente(Decimal idCliente)
        {
            Cliente objeto = new Cliente();
            Type clase = objeto.GetType();
            return (Cliente) this.Obtener(idCliente, clase);
        }

        public Empresa ObtenerEmpresa(Decimal idEmpresa)
        {
            Empresa objeto = new Empresa();
            Type clase = objeto.GetType();
            return (Empresa)this.Obtener(idEmpresa, clase);
        }

        public Direccion ObtenerDireccion(Decimal idDireccion)
        {
            Direccion objeto = new Direccion();
            Type clase = objeto.GetType();
            return (Direccion)this.Obtener(idDireccion, clase);
        }

        public Visibilidad ObtenerVisibilidad(Decimal idVisibilidad)
        {
            Visibilidad objeto = new Visibilidad();
            Type clase = objeto.GetType();
            return (Visibilidad)this.Obtener(idVisibilidad, clase);
        }

        public Publicacion ObtenerPublicacion(Decimal idPublicacion)
        {
            Publicacion objeto = new Publicacion();
            Type clase = objeto.GetType();
            return (Publicacion)this.Obtener(idPublicacion, clase);
        }

        public Object SelectFromWhere(String que, String deDonde, String param1, String param2)
        {
            query = "SELECT " + que + " FROM NET_A_CERO." + deDonde + " WHERE " + param1 + " = @" + param1;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + param1, param2));
            return QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
        }

        public Object SelectFromWhere(String que, String deDonde, String param1, Decimal param2)
        {
            query = "SELECT " + que + " FROM NET_A_CERO." + deDonde + " WHERE " + param1 + " = @" + param1;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + param1, param2));
            return QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
        }

        public DataTable SelectDataTable(String que, String deDonde)
        {
            parametros.Clear();
            command = QueryBuilder.Instance.build("SELECT " + que + " FROM " + deDonde, parametros);
            DataSet datos = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(datos);
            return datos.Tables[0];
        }

        public DataTable SelectDataTable(String que, String deDonde, String condiciones)
        {
            return this.SelectDataTableConUsuario(que, deDonde, condiciones);
        }

        public DataTable SelectDataTableConUsuario(String que, String deDonde, String condiciones)
        {
            parametros.Clear();
            parametros.Add(new SqlParameter("@idUsuario", UsuarioSesion.Usuario.id));
            command = QueryBuilder.Instance.build("SELECT " + que + " FROM " + deDonde + " WHERE " + condiciones, parametros);
            command.CommandTimeout = 0;
            DataSet datos = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(datos);
            return datos.Tables[0];
        }

        public DataTable SelectClientesParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTable("c.cli_id, u.usr_usuario Username, c.cli_nombre Nombre, c.cli_apellido Apellido, c.cli_tipo_dni 'Tipo de Documento', c.cli_dni Documento, c.cli_fecha_nac 'Fecha de Nacimiento', d.cont_mail Mail, d.cont_telefono Telefono, d.cont_calle Calle, d.cont_numero_calle Numero, d.cont_piso Piso, d.cont_depto Departamento, d.cont_codigo_postal 'Codigo postal', d.cont_localidad Localidad"
                , "NET_A_CERO.Clientes c, NET_A_CERO.Contacto d, NET_A_CERO.Usuarios u"
                , "c.cli_usr_id = u.usr_id AND u.usr_id = d.cont_id AND dado_de_baja = 0 " + filtro);
        }

        public DataTable SelectClientesParaFiltro()
        {
            return this.SelectClientesParaFiltroConFiltro("");
        }

        public DataTable SelectEmpresasParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTable("e.emp_id, u.usr_apellido Username, e.emp_razon_social 'Razon Social', e.emp_nombre_contacto 'Nombre de contacto', e.emp_cuit 'CUIT', e.emp_fecha_alta 'Fecha de creacion', d.cont_mail 'Mail', d.cont_telefono 'Telefono', d.cont_localidad Ciudad, d.cont_calle Calle, d.cont_numero_calle Numero, d.cont_piso Piso, d.cont_depto Departamento, d.cont_codigo_postal 'Codigo Postal', d.cont_localidad Localidad"
                , "NET_A_CERO.Empresas e, NET_A_CERO.Contacto d, NET_A_CERO.Usuarios u"
                , "e.emp_usr_id = u.usr_id AND u.usr_id = d.cont_id AND dado_de_baja = 0 " + filtro); //fijarse si esta bien
        }

        public DataTable SelectEmpresasParaFiltro()
        {
            return this.SelectEmpresasParaFiltroConFiltro("");
        }

        public DataTable SelectVisibilidadesParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTable("v.id, v.visib_cod Descripcion, v.visib_precio Precio, v.visib_porcentaje Porcentaje, v.visib_grado Duracion"
                , "NET_A_CERO.Visibilidad v"
                , "dado_de_baja = 0 " + filtro);
        }

        public DataTable SelectVisibilidadesParaFiltro()
        {
            return this.SelectDataTable("v.id, v.visib_cod Descripcion, v.visib_precio Precio, v.visib_porcentaje Porcentaje, v.visib_grado Duracion"
                , "NET_A_CERO.Visibilidad v"
                , "dado_de_baja = 0");
        }

        public DataTable SelectPublicacionesParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTableConUsuario("p.id, u.username Username, (SELECT descripcion FROM NET_A_CERO.Publicaciones WHERE id = p.publi_id) id, p.publi_descripcion Descripcion, p.publi_fec_inicio 'Fecha de inicio', p.publi_fec_vencimiento 'Fecha de vencimiento', r.publi_rubro_id Rubro, v.publi_visib_id Visibilidad, p.publi__preguntas 'Permite preguntas', p.publi_stock Stock, p.publi_precio Precio"
                , "NET_A_CERO.Publicaciones p, NET_A_CERO.Rubros r, NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u"
                , "p.rubro_id = r.id AND p.visib_id = v.id AND p.usr_id = u.id AND p.usr_id = @idUsuario" + filtro);
        }

        public DataTable SelectPublicacionesParaFiltro()
        {
            return this.SelectPublicacionesParaFiltroConFiltro("");
        }

        private bool ControlDeUnicidad(String query, IList<SqlParameter> parametros)
        {
            int cantidad = (int)QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
            if (cantidad > 0)
            {
                return false;
            }
            return true;
        }

        private bool pasoControlDeUnicidad(String que, String aQue, String enDonde)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO." + enDonde + " WHERE " + aQue + " = @" + aQue;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + aQue, que));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeUnicidad(String que, String aQue, String enDonde, Decimal id)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO." + enDonde + " WHERE " + aQue + " = @" + aQue + " AND id != " + id;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + aQue, que));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistro(Decimal tipoDeDocumento, String numeroDeDocumento)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Clientes WHERE cli_tipo_dni = @tipoDeDocumento AND cli_dni = @numeroDeDocumento";
            parametros.Clear();
            parametros.Add(new SqlParameter("@tipoDeDocumento", tipoDeDocumento));
            parametros.Add(new SqlParameter("@numeroDeDocumento", Convert.ToDecimal(numeroDeDocumento)));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistro(Decimal tipoDeDocumento, String numeroDeDocumento, Decimal idCliente)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Clientes WHERE cli_tipo_dni = @tipoDeDocumento AND cli_dni = @numeroDeDocumento AND cli_id != @idCliente";
            parametros.Clear();
            parametros.Add(new SqlParameter("@tipoDeDocumento", tipoDeDocumento));
            parametros.Add(new SqlParameter("@numeroDeDocumento", numeroDeDocumento));
            parametros.Add(new SqlParameter("@idCliente", idCliente));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistroDeRazonSocial(String razonSocial)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_razon_social = @razonSocial";
            parametros.Clear();
            parametros.Add(new SqlParameter("@razonSocial", razonSocial));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistroDeRazonSocial(String razonSocial, Decimal idEmpresa)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_razon_social = @razonSocial AND emp_id != @idEmpresa";
            parametros.Clear();
            parametros.Add(new SqlParameter("@razonSocial", razonSocial));
            parametros.Add(new SqlParameter("@idEmpresa", idEmpresa));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistroDeCuit(String cuit)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_cuit = @cuit";
            parametros.Clear();
            parametros.Add(new SqlParameter("@cuit", cuit));
            return ControlDeUnicidad(query, parametros);
        }

        private bool pasoControlDeRegistroDeCuit(String cuit, Decimal idEmpresa)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_cuit = @cuit AND emp_id != @idEmpresa";
            parametros.Clear();
            parametros.Add(new SqlParameter("@cuit", cuit));
            parametros.Add(new SqlParameter("@idEmpresa", idEmpresa));
            return ControlDeUnicidad(query, parametros);
        }

    }
}