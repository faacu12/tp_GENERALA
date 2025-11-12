using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Sesion
    {
        private static List<Usuario> Usuarios = new List<Usuario>(2);
        public void Agregar(Usuario u)
        {
            
                if (u != null && Usuarios.Count < 2)
                {
                    Usuarios.Add(u);
                }
                else
                {
                    throw new Exception("Ya hay 2 jugadores en la partida!");
                }
            
        }

        public static void Eliminar(Usuario u)
        {
           
                if (u != null && Usuarios.Count > 0)
                {
                    Usuario aeliminar = Usuarios.Where(x => x.Id == u.Id).FirstOrDefault();
                    Usuarios.Remove(aeliminar);
                }
                else
                {
                    throw new Exception("Error!");
                }
           
        }
        public Usuario Get(int indice = 0)
        {
            return (indice >= 0 && indice < Usuarios.Count) ? Usuarios[indice] : null;
        }
    }
}
