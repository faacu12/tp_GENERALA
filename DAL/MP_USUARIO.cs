using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_USUARIO : MAPPER<BE.Usuario>
    {
        public override int Agregar(Usuario objeto)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.Nombre));
            parametros.Add(acceso.CrearParametro("@contraseña", objeto.Contraseña));
            int res = acceso.Escribir("USUARIO_INSERTAR", parametros);
            acceso.Cerrar();
            return res;
        }
        public Usuario ValidarCredenciales(string nombreUsuario, string contraseña)
        {
            acceso = new Acceso();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", nombreUsuario));
            parametros.Add(acceso.CrearParametro("@contraseña", contraseña));

            DataTable dt = acceso.Leer("USUARIO_VALIDAR", parametros);
            acceso.Cerrar();

            if (dt.Rows.Count > 0)
            {
                // Mapear el DataRow a Usuario
                Usuario usuario = new Usuario();
                usuario.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                usuario.Nombre = dt.Rows[0]["NombreUsuario"].ToString();
                usuario.Contraseña = dt.Rows[0]["Contraseña"].ToString();
                return usuario;
            }

            return null; // Usuario no encontrado
        }

        public override int Eliminar(Usuario objeto)
        {
            throw new NotImplementedException();
        }

        public override List<Usuario> Listar()
        {
            throw new NotImplementedException();
        }

        public override int Modificar(Usuario objeto)
        {
            throw new NotImplementedException();
        }
    }
}
