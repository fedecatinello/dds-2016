using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MercadoEnvio.Objetos;
using MercadoEnvio.Exceptions;

namespace MercadoEnvio.ABM_Empresa
{
    public partial class AgregarEmpresa : Form
    {
        private String username;
        private String contrasena;
        private DBMapper comunicador = new DBMapper();
        private int idContacto;
        private int idUsuario;
        private int idEmpresa;

        public AgregarEmpresa(String username, String contrasena)
        {
            InitializeComponent();
            this.username = username;
            this.contrasena = contrasena;
            this.idContacto = 0;
            this.idUsuario = 0;
        }

        private void AgregarEmpresa_Load(object sender, EventArgs e)
        {

        }

        private void button_Guardar_Click(object sender, EventArgs e)
        {
            // Guarda en variables todos los campos de entrada
            String razonSocial = textBox_RazonSocial.Text;
            String ciudad = textBox_Ciudad.Text;
            String cuit = textBox_CUIT.Text;
            String nombreDeContacto = textBox_NombreDeContacto.Text;
            String rubro = textBox_Rubro.Text;
            DateTime fechaDeCreacion;
            DateTime.TryParse(textBox_FechaDeCreacion.Text, out fechaDeCreacion);

            String mail = textBox_Mail.Text;
            String telefono = textBox_Telefono.Text;
            String calle = textBox_Calle.Text;
            String numeroCalle = textBox_Numero.Text;
            String piso = textBox_Piso.Text;
            String departamento = textBox_Departamento.Text;
            String localidad = textBox_Localidad.Text;
            String codigoPostal = textBox_CodigoPostal.Text;
            

            // Crea una contacto y se guarda su id
            Contacto contacto = new Contacto();
            try
            {
                contacto.setMail(mail);
                contacto.setTelefono(telefono);
                contacto.SetCalle(calle);
                contacto.SetNumeroCalle(numeroCalle);
                contacto.SetPiso(piso);
                contacto.SetDepartamento(departamento);
                contacto.SetLocalidad(localidad);
                contacto.SetCodigoPostal(codigoPostal);
                
            }
            catch (CampoVacioException exception)
            {
                MessageBox.Show("Falta completar campo: " + exception.Message);
                return;
            }
            catch (FormatoInvalidoException exception)
            {
                MessageBox.Show("Los datos fueron mal ingresados en: " + exception.Message);
                return;
            }
            
            // Controla que no se haya creado ya la contacto
            if (this.idContacto == 0)
            {
                this.idContacto = comunicador.CrearContacto(contacto);
            }
            
            // Crea empresa
            try
            {
                Empresas empresa = new Empresas();
                Usuarios usuario = new Usuarios();

                empresa.SetRazonSocial(razonSocial);
                empresa.SetCiudad(ciudad);
                empresa.SetCuit(cuit);
                empresa.SetNombreDeContacto(nombreDeContacto);
                empresa.SetRubro(rubro);
                empresa.SetFechaDeCreacion(fechaDeCreacion);
                empresa.SetIdUsuario(idUsuario);
                usuario.SetActivo(true);
                empresa.SetIdContacto(idContacto);
                
                idEmpresa = comunicador.CrearEmpresa(empresa);
                if (idEmpresa > 0) MessageBox.Show("Se agrego la empresa correctamente");
            }
            catch (CampoVacioException exceptionCampoVacio)
            {
                MessageBox.Show("Falta completar campo: " + exceptionCampoVacio.Message);
                return;
            }
            catch (FormatoInvalidoException exceptionFormato)
            {
                MessageBox.Show("Los datos fueron mal ingresados en: " + exceptionFormato.Message);
                return;
            }
            catch (TelefonoYaExisteException exceptionTelefono)
            {
                MessageBox.Show("Telefono ya existe");
                return;
            }
            catch (CuitYaExisteException exceptionCuit)
            {
                MessageBox.Show("Cuit ya existe");
                return;
            }
            catch (RazonSocialYaExisteException exceptionRazonSocial)
            {
                MessageBox.Show("RazonSocial ya existe");
                return;
            }
            catch (FechaPasadaException exceptionFecha)
            {
                MessageBox.Show("Fecha no valida");
                return;
            }

            // Si la empresa lo crea el admin, crea un nuevo usuario predeterminado.
            if (idUsuario == 0)
            {
                idUsuario = CrearUsuario();
                Boolean resultado = comunicador.AsignarUsuarioAEmpresa(idEmpresa, idUsuario);
                if (resultado) MessageBox.Show("El usuario fue creado correctamente");
            }
            /*
             ------------- SOLO LOS ADMINISTRADORES PUEDEN CREAR USUARIOS-----------------
            if (UsuarioSesion.Usuario.rol != "Administrador")
            {
                UsuarioSesion.Usuario.rol = "Empresa";
                UsuarioSesion.Usuario.nombre = username;
                UsuarioSesion.Usuario.id = idUsuario;
            }
            */
            comunicador.AsignarRolAUsuario(this.idUsuario, "Empresa");

            VolverAlMenuPrincipal();
        }

        private int CrearUsuario()
        {
            /*
             ------------- SOLO LOS ADMINISTRADORES PUEDEN CREAR USUARIOS-----------------
            if (username == "clienteCreadoPorAdmin")
            {
                return comunicador.CrearUsuario();
            }
            else
            {*/
                return comunicador.CrearUsuarioConValores(username, contrasena);
            //}
        }

        private void button_Limpiar_Click(object sender, EventArgs e)
        {
            textBox_RazonSocial.Text = "";
            textBox_Ciudad.Text = "";
            textBox_CUIT.Text = "";
            textBox_NombreDeContacto.Text = "";
            textBox_Rubro.Text = "";
            textBox_FechaDeCreacion.Text = "";
            textBox_Mail.Text = "";
            textBox_Telefono.Text = "";
            textBox_Calle.Text = "";
            textBox_Numero.Text = "";
            textBox_Piso.Text = "";
            textBox_Departamento.Text = "";
            textBox_CodigoPostal.Text = "";
            textBox_Localidad.Text = "";
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            if (UsuarioSesion.Usuario.rol != "Administrador")
            {
                this.Hide();
                new Registro_de_Usuario.RegistrarUsuario().ShowDialog();
                this.Close();
            }
            else
            {
                VolverAlMenuPrincipal();
            }
        }

        private void VolverAlMenuPrincipal()
        {
            this.Hide();
            new MenuPrincipal().ShowDialog();
            this.Close();
        }

        private void button_FechaDeCreacion_Click(object sender, EventArgs e)
        {
            monthCalendar_FechaDeCreacion.Visible = true;
        }

        private void monthCalendar_FechaDeCreacion_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            textBox_FechaDeCreacion.Text = e.Start.ToShortDateString();
            monthCalendar_FechaDeCreacion.Visible = false;
        }
    }
}