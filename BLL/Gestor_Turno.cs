using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Gestor_Turno
    {
        private List<Usuario> usuarios = new List<Usuario>();
        public List<Usuario> Usuarios
        {
            get { return usuarios; }
        }
        public int indice;

        public Usuario UsuarioActual
        {
            get
            {
                if (usuarios.Count > 0 && indice >= 0 && indice < usuarios.Count)
                    return Usuarios[indice];
                return null;
            }
        }

        public void Cambiar()
        {
            indice++;
            if (indice == Usuarios.Count)
            {
                indice = 0;
            }
        }

        public void InicializarJugadores(List<Usuario> usuariosenpartida)
        {
            usuarios.Clear();
            if (usuariosenpartida != null)
            {
                foreach (var jugador in usuariosenpartida)
                {
                    if (jugador != null)
                        usuarios.Add(jugador);
                }
            }
        }
    }
}

