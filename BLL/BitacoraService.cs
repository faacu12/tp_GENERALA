using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraService
    {
        private Acceso acceso = new DAL.Acceso();
        private XmlBitacora xmlBitacora = new DAL.XmlBitacora();

        private void Registrar(string tipo, int? usuarioId, string descripcion)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Tip", tipo));
            parametros.Add(acceso.CrearParametro("@Desc", descripcion ?? string.Empty));
            parametros.Add(
                usuarioId.HasValue
                    ? acceso.CrearParametro("@idus", usuarioId.Value)
                    : new SqlParameter("@idus", DBNull.Value) { DbType = DbType.Int32, IsNullable = true }
            );
            acceso.Escribir("Bitacora_Insertar", parametros);
            acceso.Cerrar();
            
        }

        // XML 
        public void RegistrarMovimiento(string accion, Usuario jugador, string categoria, int puntos, int turno)
        {
            xmlBitacora.RegistrarMovimiento(accion, jugador?.Id, jugador?.Nombre, categoria, puntos, turno);
        }
        public void LimpiarBitacora()
        {
            xmlBitacora.Limpiar();
        }
        public void RegistrarLogin(Usuario usuario)
        {
            Registrar("InicioSesión", usuario?.Id, $"Inicio de sesión de {usuario?.Nombre}");
        }
        public void RegistrarLogout(Usuario usuario)
        {
            Registrar("CierreSesión", usuario?.Id, $"Cierre de sesión de {usuario?.Nombre}");
        }
        public void RegistrarInicio(List<Usuario> jugadores)
        {
            string nombres = string.Join(", ", jugadores.Select(j => j.Nombre));
            Registrar("InicioPartida", null, $"Se comenzó una partida. Los jugadores son {nombres}");
        }
        public void RegistrarFin(List<Usuario> jugadores, Usuario ganador = null)
        {
            string nombres = string.Join(" y ", jugadores.Select(j => j.Nombre));
            string desc = ganador != null
                ? $"Se finalizó la partida entre {nombres}. El ganador fue {ganador.Nombre}"
                : $"Se finalizó la partida entre {nombres}. La partida terminó en empate";
            Registrar("FinPartida", null, desc);
        }
        public void RegistrarCreacionUsuario(Usuario usuario)
        {
            Registrar("Creacion De Usuari", usuario.Id, $"Se creó la cuenta de {usuario.Nombre}");
        }

        public DataTable ConsultarMovimientos(DateTime? desde = null, DateTime? hasta = null, int? usuarioId = null, string accion = null)
        {
            return xmlBitacora.Consultar(desde, hasta, "Movimiento", usuarioId, accion);
        }
    }
}
