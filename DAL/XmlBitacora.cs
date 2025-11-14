using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DAL
{
    public class XmlBitacora
    {
        private readonly string _rutaArchivo;
        private readonly DataSet _ds = new DataSet("BitacoraDS");

        public XmlBitacora(string ruta = null)
        {
            _rutaArchivo = ruta ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bitacora", "Bitacora.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaArchivo));
            Cargar();
        }

        public void Registrar(string tipoEvento, int? idUsuario, string descripcion)
        {
            Cargar();
            DataTable tabla = _ds.Tables["Bitacora"];
            DataRow fila = tabla.NewRow();
            fila["FechaHora"] = DateTime.Now;
            fila["Tipo"] = tipoEvento ?? string.Empty;
            fila["UsuarioId"] = (object)idUsuario ?? DBNull.Value;
            fila["Accion"] = DBNull.Value;
            fila["Jugador"] = DBNull.Value;
            fila["Categoria"] = DBNull.Value;
            fila["Puntos"] = DBNull.Value;
            fila["Turno"] = DBNull.Value;
            fila["Mensaje"] = descripcion ?? string.Empty;
            tabla.Rows.Add(fila);
            Guardar();
        }

        public void RegistrarMovimiento(string accion, int? usuarioId, string jugadorNombre, string categoria, int puntos, int turno)
        {
            Cargar();
            DataTable tabla = _ds.Tables["Bitacora"];
            DataRow fila = tabla.NewRow();
            fila["FechaHora"] = DateTime.Now;
            fila["Tipo"] = "Movimiento";
            fila["UsuarioId"] = (object)usuarioId ?? DBNull.Value;
            fila["Accion"] = accion ?? string.Empty;
            fila["Jugador"] = jugadorNombre ?? string.Empty;
            fila["Categoria"] = categoria ?? string.Empty;
            fila["Puntos"] = puntos;
            fila["Turno"] = turno;
            fila["Mensaje"] = string.Empty;
            tabla.Rows.Add(fila);
            Guardar();
        }

        public DataTable Consultar(DateTime? desde = null, DateTime? hasta = null, string tipo = null, int? usuarioId = null, string accion = null)
        {
            Cargar();
            DataTable tabla = _ds.Tables["Bitacora"];
            var q = tabla.AsEnumerable();

            if (desde.HasValue) q = q.Where(r => r.Field<DateTime>("FechaHora") >= desde.Value);
            if (hasta.HasValue) q = q.Where(r => r.Field<DateTime>("FechaHora") <= hasta.Value);
            if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(r => string.Equals(r.Field<string>("Tipo") ?? "", tipo, StringComparison.OrdinalIgnoreCase));
            if (usuarioId.HasValue) q = q.Where(r => !r.IsNull("UsuarioId") && r.Field<int>("UsuarioId") == usuarioId.Value);
            if (!string.IsNullOrWhiteSpace(accion)) q = q.Where(r => string.Equals(r.Field<string>("Accion") ?? "", accion, StringComparison.OrdinalIgnoreCase));

            q = q.OrderByDescending(r => r.Field<DateTime>("FechaHora"));

            DataTable resultado = tabla.Clone();
            foreach (var r in q) resultado.ImportRow(r);
            return resultado;
        }

        public void Limpiar()
        {
            _ds.Reset();
            CrearEsquema();
            Guardar();
        }

        private void Cargar()
        {
            _ds.Reset();
            if (File.Exists(_rutaArchivo))
            {
                _ds.ReadXml(_rutaArchivo, XmlReadMode.ReadSchema);
                if (!_ds.Tables.Contains("Bitacora"))
                {
                    CrearEsquema();
                    Guardar();
                }
            }
            else
            {
                CrearEsquema();
                Guardar();
            }
        }

        private void Guardar()
        {
            _ds.WriteXml(_rutaArchivo, XmlWriteMode.WriteSchema);
        }

        private void CrearEsquema()
        {
            DataTable tabla = new DataTable("Bitacora");
            tabla.Columns.Add(new DataColumn("FechaHora", typeof(DateTime)));
            tabla.Columns.Add(new DataColumn("Tipo", typeof(string)));
            tabla.Columns.Add(new DataColumn("UsuarioId", typeof(int)) { AllowDBNull = true });
            tabla.Columns.Add(new DataColumn("Accion", typeof(string)));
            tabla.Columns.Add(new DataColumn("Jugador", typeof(string)));
            tabla.Columns.Add(new DataColumn("Categoria", typeof(string)));
            tabla.Columns.Add(new DataColumn("Puntos", typeof(int)) { AllowDBNull = true });
            tabla.Columns.Add(new DataColumn("Turno", typeof(int)) { AllowDBNull = true });
            tabla.Columns.Add(new DataColumn("Mensaje", typeof(string)));
            _ds.Tables.Add(tabla);
        }
    }
}