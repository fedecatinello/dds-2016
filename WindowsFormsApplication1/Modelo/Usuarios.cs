using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercadoEnvio.Modelo
{
    class Usuarios
    {
        private Decimal id;
        private String username;
        private String password;
        private Boolean activo;
        private Boolean is_admin;

        public void SetId(Decimal id)
        {
            this.id = id;
        }

        public Decimal GetId()
        {
            return this.id;
        }

        public void SetUsername(String username)
        {
            this.username = username;
        }

        public String GetUsername()
        {
            return this.username;
        }

        public void SetPassword(String password)
        {
            this.password = password;
        }

        public String GetPassword()
        {
            return this.password;
        }

        public void SetActivo(Boolean activo)
        {
            this.activo = activo;
        }

        public Boolean GetActivo()
        {
            return this.activo;
        }

        public void Setis_admin(Boolean activo)
        {
            this.is_admin = activo;
        }

        public Boolean Getis_admin()
        {
            return this.is_admin;
        }

    }
}