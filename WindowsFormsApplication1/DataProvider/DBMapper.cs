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
    class DBMapper
    {
        private String query;
        private IList<SqlParameter> parametros = new List<SqlParameter>();
        private SqlParameter parametroOutput;
        private SqlCommand command;

        
        /*
        *
        *   GENERIC CRUD
        *
        */


        public int Crear(Comunicable objeto)
        {
            query = objeto.GetQueryCrear();
            parametros.Clear();
            parametros = objeto.GetParametros();
            parametroOutput = new SqlParameter("@id", SqlDbType.Int);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (int)parametroOutput.Value;
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

       /* public Boolean Eliminar(String idParameter, Decimal id, String enDonde)
        {
            query = "UPDATE NET_A_CERO." + enDonde + " SET dado_de_baja = 1 WHERE id = @id";
            parametros.Clear();
            parametros.Add(new SqlParameter("@"+idParameter, id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }
        * */
        
        /*
        *
        *   CREATE TABLE QUERYS
        *
        */


        /** Usuarios **/

        public int CrearUsuario()
        {
            query = "NET_A_CERO.pr_crear_usuario";
            parametros.Clear();
            parametroOutput = new SqlParameter("@usr_id", SqlDbType.Int);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (int)parametroOutput.Value;
        }

        public int CrearUsuarioConValores(String username, String password)
        {
            query = "NET_A_CERO.pr_crear_usuario_con_valores";
            parametros.Clear();
            parametroOutput = new SqlParameter("@usuario_id", SqlDbType.Int);
            parametroOutput.Direction = ParameterDirection.Output;
            parametros.Add(new SqlParameter("@username", username));
            parametros.Add(new SqlParameter("@password", HashSha256.getHash(password)));
            parametros.Add(new SqlParameter("@is_admin", "0"));
            parametros.Add(parametroOutput);
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            return (int)parametroOutput.Value;
        }

        /** Clientes **/

        public int CrearCliente(Clientes cliente)
        {

            if (!esClienteUnico(cliente.GetTipoDeDocumento(), cliente.GetNumeroDeDocumento()))
                throw new ClienteYaExisteException();

            return this.Crear(cliente);

        }

        /** Empresas **/

        public int CrearEmpresa(Empresas empresa)
        {
            if (!esEmpresaCuitUnica(empresa.GetCuit()))
                throw new CuitYaExisteException();

            if (!esEmpresaRazonUnica(empresa.GetRazonSocial()))
                throw new RazonSocialYaExisteException();

            return this.Crear(empresa);
        }

        /** Contacto **/

        public int CrearContacto(Contacto contacto)
        {
            return this.Crear(contacto);
        }

        /** Publicaciones **/

        public Decimal CrearPublicacion(Publicacion publicacion)
        {
            return this.Crear(publicacion);
        }

        /** Visibilidad **/

        public Decimal CrearVisibilidad(Visibilidad visibilidad)
        {
            if (!esVisibilidadUnica(visibilidad.GetDescripcion()))
                throw new VisibilidadYaExisteException();

            return this.Crear(visibilidad);
        }


        /*
        *
        *   GET TABLE QUERYS
        *
        */


        /** Usuarios **/

        public Usuarios ObtenerUsuario(int idUsuario)
        {
            Usuarios objeto = new Usuarios();
            Type clase = objeto.GetType();
            return (Usuarios)this.Obtener(idUsuario, clase);
        }

        /** Clientes **/

        public Clientes ObtenerCliente(int idCliente)
        {
            Clientes objeto = new Clientes();
            Type clase = objeto.GetType();
            return (Clientes)this.Obtener(idCliente, clase);
        }

        /** Empresas **/

        public Empresas ObtenerEmpresa(int idEmpresa)
        {
            Empresas objeto = new Empresas();
            Type clase = objeto.GetType();
            return (Empresas)this.Obtener(idEmpresa, clase);
        }

        /** Contacto **/

        public Contacto ObtenerContacto(int idContacto)
        {
            Contacto objeto = new Contacto();
            Type clase = objeto.GetType();
            return (Contacto)this.Obtener(idContacto, clase);
        }

        /** Visibilidad **/

        public Visibilidad ObtenerVisibilidad(Decimal idVisibilidad)
        {
            Visibilidad objeto = new Visibilidad();
            Type clase = objeto.GetType();
            return (Visibilidad)this.Obtener(idVisibilidad, clase);
        }

        /** Publicaciones **/

        public Publicacion ObtenerPublicacion(Decimal idPublicacion)
        {
            Publicacion objeto = new Publicacion();
            Type clase = objeto.GetType();
            return (Publicacion)this.Obtener(idPublicacion, clase);
        }


        /* 
        * 
        *   DELETE QUERYS
        *
        */


        /** Clientes **/

        public Boolean EliminarCliente(int id, String enDonde)
        {
            query = "UPDATE NET_A_CERO." + enDonde + " SET cli_activo = 0 WHERE cli_id = @id";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Boolean EliminarEmpresa(Decimal id, String enDonde)
        {
            query = "UPDATE NET_A_CERO." + enDonde + " SET emp_activo = 0 WHERE emp_id = @id";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        public Boolean EliminarVisibilidad(Decimal id, String enDonde)
        {
            query = "UPDATE NET_A_CERO." + enDonde + " SET visib_activo = 0 WHERE visib_id = @id";
            parametros.Clear();
            parametros.Add(new SqlParameter("@id", id));
            int filasAfectadas = QueryBuilder.Instance.build(query, parametros).ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }

        
        /* 
        * 
        *   ASSIGN QUERYS
        *
        */

        public Boolean AsignarUsuarioACliente(Decimal idCliente, Decimal idUsuario)
        {
            query = "UPDATE NET_A_CERO.Clientes SET cli_usr_id = @idUsuario WHERE cli_id = @idCliente";
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
            query = "UPDATE NET_A_CERO.Empresas SET emp_usr_id = @idUsuario WHERE emp_id = @idEmpresa";
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
            Decimal idRol = Convert.ToDecimal(this.SelectFromWhere("rol_id", "Roles", "rol_nombre", rol));
            query = "NET_A_CERO.pr_agregar_rol_a_usuario";
            parametros.Clear();
            parametros.Add(new SqlParameter("@usuario_id", idUsuario));
            parametros.Add(new SqlParameter("@rol_id", idRol));
            command = QueryBuilder.Instance.build(query, parametros);
            command.CommandType = CommandType.StoredProcedure;
            int filasAfectadas = command.ExecuteNonQuery();
            if (filasAfectadas == 1) return true;
            return false;
        }


        /* 
        * 
        *   SELECT QUERYS
        *
        */


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

        /*
        *
        *   SELECT TABLE QUERYS
        *
        */

        /** Clientes **/

        public DataTable SelectClientesParaFiltro()
        {
            return this.SelectClientesParaFiltroConFiltro("");
        }

        public DataTable SelectClientesParaFiltroConFiltro(String filtro)
        {

            return this.SelectDataTable("cli.cli_id, usr.usr_usuario Username, cli.cli_nombre Nombre, cli.cli_apellido Apellido, cli.cli_dni Documento, cli.cli_tipo_dni 'Tipo de Documento', cli.cli_fecha_nac 'Fecha de Nacimiento', cli.cli_activo 'Habilitado', cont.cont_mail Mail, cont.cont_telefono Telefono, cont.cont_calle Calle, cont.cont_numero_calle 'Numero Calle', cont.cont_piso Piso, cont.cont_depto Departamento, cont.cont_localidad Localidad, cont.cont_codigo_postal 'Codigo Postal' "
                , "NET_A_CERO.Clientes cli, NET_A_CERO.Contacto cont, NET_A_CERO.Usuarios usr"
                , "cli.cli_usr_id = usr.usr_id AND cli.cli_cont_id = cont.cont_id  AND cli.cli_activo = 1" + filtro);
        }

        /** Empresas **/

        public DataTable SelectEmpresasParaFiltro()
        {
            return this.SelectEmpresasParaFiltroConFiltro("");
        }

        public DataTable SelectEmpresasParaFiltroConFiltro(String filtro)
        {
              return this.SelectDataTable("emp.emp_id, usr.usr_usuario Username, emp.emp_razon_social 'Razon Social', emp.emp_ciudad Ciudad, emp.emp_cuit 'CUIT', emp.emp_nombre_contacto 'Nombre Contacto', (SELECT rubro_desc_larga FROM NET_A_CERO.Rubros WHERE rubro_id = emp.emp_rubro) 'Rubro', emp.emp_fecha_alta 'Fecha Alta', emp.emp_activo 'Habilitado', cont.cont_mail Mail, cont.cont_telefono Telefono, cont.cont_calle Calle, cont.cont_numero_calle 'Numero Calle', cont.cont_piso Piso, cont.cont_depto Departamento, cont.cont_localidad Localidad, cont.cont_codigo_postal 'Codigo Postal' "
                , "NET_A_CERO.Empresas emp, NET_A_CERO.Contacto cont, NET_A_CERO.Usuarios usr"
                , "emp.emp_usr_id = usr.usr_id AND emp.emp_cont_id = cont.cont_id AND emp.emp_activo = 1" + filtro);
        }

        /** Visibilidad **/

        public DataTable SelectVisibilidadesParaFiltro()
        {
            return this.SelectVisibilidadesParaFiltroConFiltro("");
        }

        public DataTable SelectVisibilidadesParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTable("v.visib_id Codigo, v.visib_desc Descripcion, v.visib_grado Grado, v.visib_precio Precio, v.visib_porcentaje Porcentaje, v.visib_envios 'Soporta Envios', v.visib_activo 'Habilitado'"
                , "NET_A_CERO.Visibilidad v"
                , "v.visib_desc IS NOT NULL" + filtro);
        }

        /** Publicaciones **/

        public DataTable SelectPublicacionesParaFiltro()
        {
            return this.SelectPublicacionesParaFiltroConFiltro("");
        }

        public DataTable SelectPublicacionesParaFiltroConFiltro(String filtro)
        {
            return this.SelectDataTableConUsuario("p.publi_id, u.usr_usuario Usuario, p.publi_descripcion Descripcion, p.publi_fec_inicio 'Fecha de inicio', p.publi_fec_vencimiento 'Fecha de vencimiento', r.rubro_desc_larga Rubro, v.visib_desc Visibilidad, p.publi_preguntas 'Permite preguntas', p.publi_stock Stock, p.publi_precio Precio"
                , "NET_A_CERO.Publicaciones p, NET_A_CERO.Rubros r, NET_A_CERO.Visibilidad v, NET_A_CERO.Usuarios u, NET_A_CERO.Rubro_x_Publicacion rxp"
                , "rxp.rubro_id = r.rubro_id AND rxp.publi_id=p.publi_id AND p.publi_visib_id = v.visib_id AND p.publi_usr_id = u.usr_id AND p.publi_usr_id = @idUsuario" + filtro);
        }

        
        /*
        *
        *   TABLE UNIQUE CONTROL 
        *
        */

        private bool ControlDeUnicidad(String query, IList<SqlParameter> parametros)
        {
            int cantidad = (int)QueryBuilder.Instance.build(query, parametros).ExecuteScalar();
            if (cantidad > 0)
            {
                return false;
            }
            return true;
        }

        //  TODO: QUITAR SI NO SE UTILIZA

        /*private bool esUnico(String que, String aQue, String enDonde)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO." + enDonde + " WHERE " + aQue + " = @" + aQue;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + aQue, que));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esUnico(String que, String aQue, String enDonde, Decimal id)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO." + enDonde + " WHERE " + aQue + " = @" + aQue + " AND id != " + id;
            parametros.Clear();
            parametros.Add(new SqlParameter("@" + aQue, que));
            return ControlDeUnicidad(query, parametros);
        }
        */

        /** Clientes **/

        private bool esClienteUnico(String tipoDeDocumento, String numeroDeDocumento)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Clientes WHERE cli_tipo_dni = @tipoDeDocumento AND cli_dni = @numeroDeDocumento";
            parametros.Clear();
            parametros.Add(new SqlParameter("@tipoDeDocumento", tipoDeDocumento));
            parametros.Add(new SqlParameter("@numeroDeDocumento", Convert.ToDecimal(numeroDeDocumento)));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esClienteUnico(Decimal tipoDeDocumento, String numeroDeDocumento, int idCliente)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Clientes WHERE cli_tipo_dni = @tipoDeDocumento AND cli_dni = @numeroDeDocumento AND cli_id != @idCliente";
            parametros.Clear();
            parametros.Add(new SqlParameter("@tipoDeDocumento", tipoDeDocumento));
            parametros.Add(new SqlParameter("@numeroDeDocumento", numeroDeDocumento));
            parametros.Add(new SqlParameter("@idCliente", idCliente));
            return ControlDeUnicidad(query, parametros);
        }

        /** Empresas **/

        private bool esEmpresaRazonUnica(String razonSocial)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_razon_social = @razonSocial";
            parametros.Clear();
            parametros.Add(new SqlParameter("@razonSocial", razonSocial));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esEmpresaRazonUnica(String razonSocial, int idEmpresa)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_razon_social = @razonSocial AND emp_id != @idEmpresa";
            parametros.Clear();
            parametros.Add(new SqlParameter("@razonSocial", razonSocial));
            parametros.Add(new SqlParameter("@idEmpresa", idEmpresa));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esEmpresaCuitUnica(String cuit)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_cuit = @cuit";
            parametros.Clear();
            parametros.Add(new SqlParameter("@cuit", cuit));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esEmpresaCuitUnica(String cuit, int idEmpresa)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Empresas WHERE emp_cuit = @cuit AND emp_id != @idEmpresa";
            parametros.Clear();
            parametros.Add(new SqlParameter("@cuit", cuit));
            parametros.Add(new SqlParameter("@idEmpresa", idEmpresa));
            return ControlDeUnicidad(query, parametros);
        }

        /** Visibilidad **/

        private bool esVisibilidadUnica(String descripcion)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Visibilidad WHERE visib_desc = @desc";
            parametros.Clear();
            parametros.Add(new SqlParameter("@desc", descripcion));
            return ControlDeUnicidad(query, parametros);
        }

        private bool esVisibilidadUnica(String descripcion, Decimal codVisibilidad)
        {
            query = "SELECT COUNT(*) FROM NET_A_CERO.Visibilidad WHERE visib_desc = @desc AND visib_id != @cod";
            parametros.Clear();
            parametros.Add(new SqlParameter("@desc", descripcion));
            parametros.Add(new SqlParameter("@cod", codVisibilidad));
            return ControlDeUnicidad(query, parametros);
        }

    }
}