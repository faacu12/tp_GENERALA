using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class USUARIO
    {
        DAL.MP_USUARIO mp = new DAL.MP_USUARIO();
        public void Grabar(BE.Usuario usuario)
        {
            mp.Agregar(usuario);    
        }
        public BE.Usuario ValidarLogin(string nombreUsuario, string contraseña)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new Exception("El nombre de usuario no puede estar vacío");

            if (string.IsNullOrWhiteSpace(contraseña))
                throw new Exception("La contraseña no puede estar vacía");

            BE.Usuario usuario = mp.ValidarCredenciales(nombreUsuario, contraseña);

            if (usuario == null)
                throw new Exception("Usuario o contraseña incorrectos");

            return usuario;
        }
    }
}
