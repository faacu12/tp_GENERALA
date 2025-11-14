using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace BLL
{
    public class BitacoraService
    {
        // Renombrado: accesoDatos en lugar de 'acceso'
        private Acceso accesoDatos = new DAL.Acceso();

        // Renombrado: bitacoraXml en lugar de 'xmlBitacora'
        private XmlBitacora bitacoraXml;

        // Renombrado: rutaArchivoBitacoraActual en lugar de '_rutaActual'
        private string rutaArchivoBitacoraActual;

        // Crear archivo XML por partida: Bitacora\partida_yyyyMMdd_HHmmss.xml
        public void NuevaPartidaBitacora(DateTime fechaInicio, string sufijo = null)
        {
            string carpetaBitacora = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bitacora");
            Directory.CreateDirectory(carpetaBitacora);

            string selloTiempo = fechaInicio.ToString("yyyyMMdd_HHmmss");
            string nombreArchivo = string.IsNullOrWhiteSpace(sufijo)
                ? $"partida_{selloTiempo}.xml"
                : $"partida_{selloTiempo}_{sufijo}.xml";

            rutaArchivoBitacoraActual = Path.Combine(carpetaBitacora, nombreArchivo);

            bitacoraXml = new XmlBitacora(rutaArchivoBitacoraActual);
        }

        public string ObtenerRutaBitacoraActual() => rutaArchivoBitacoraActual;

   
        private void Registrar(string tipoEvento, int? idUsuario, string descripcionEvento)
        {
            accesoDatos.Abrir();

            var parametros = new List<SqlParameter>
            {
                accesoDatos.CrearParametro("@Tip", tipoEvento),
                accesoDatos.CrearParametro("@Desc", descripcionEvento ?? string.Empty),
                idUsuario.HasValue
                    ? accesoDatos.CrearParametro("@idus", idUsuario.Value)
                    : new SqlParameter("@idus", DBNull.Value) { DbType = DbType.Int32, IsNullable = true }
            };

            accesoDatos.Escribir("Bitacora_Insertar", parametros);

            // Espejo en XML (evento general)
            bitacoraXml?.Registrar(tipoEvento, idUsuario, descripcionEvento);
        }

        // Registrar movimiento de partida en XML (anotar/tachar)
        // Renombrado de parámetros a español claro
        public void RegistrarMovimiento(string accionRealizada, Usuario jugador, string nombreCategoria, int puntosObtenidos, int numeroTurno)
        {
            bitacoraXml?.RegistrarMovimiento(accionRealizada, jugador?.Id, jugador?.Nombre, nombreCategoria, puntosObtenidos, numeroTurno);
        }

       
        public DataTable ConsultarMovimientos(DateTime? desde = null, DateTime? hasta = null, int? idUsuario = null, string accion = null)
        {
            return bitacoraXml?.Consultar(desde, hasta, "Movimiento", idUsuario, accion) ?? new DataTable();
        }

        // Eventos generales SQL + XML (mantengo nombres públicos para no romper la GUI)
        public void RegistrarLogin(Usuario usuario)
        {
            Registrar("InicioSesión", usuario?.Id, $"Inicio de sesión de {usuario?.Nombre}");
        }
        public void RegistrarCreacionUsuario(Usuario usuario)
        {
            Registrar("CreacionUsuario", usuario.Id, $"Cierre de sesion de {usuario.Nombre}");
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
            string descripcion = ganador != null
                ? $"Se finalizó la partida entre {nombres}. El ganador fue {ganador.Nombre}"
                : $"Se finalizó la partida entre {nombres}. La partida terminó en empate";
            Registrar("FinPartida", null, descripcion);
        }

        // Cerrar contexto de bitácora (no borra archivos)
        public void LimpiarBitacora()
        {
            bitacoraXml = null;
            rutaArchivoBitacoraActual = null;
        }
    }
}
