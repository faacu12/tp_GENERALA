using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class XmlBitacora
    {
        DataSet ds = new DataSet();
        public void Escribir(string mensaje)
        {
            string rutaArchivo = "Bitacora.xml";
            if (!File.Exists(rutaArchivo))
            {
                // Crear el archivo XML si no existe
                XDocument nuevoDoc = new XDocument(
                    new XElement("Bitacora")
                );
                nuevoDoc.Save(rutaArchivo);
            }
            // Cargar el archivo XML existente
            XDocument doc = XDocument.Load(rutaArchivo);
            // Agregar un nuevo elemento de entrada de bitácora
            XElement nuevaEntrada = new XElement("Entrada",
                new XElement("FechaHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Mensaje", mensaje)
            );
            doc.Root.Add(nuevaEntrada);
            doc.Save(rutaArchivo);
        }
        public void Leer()
        {
            string rutaArchivo = "Bitacora.xml";
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("El archivo de bitácora no existe.");
            }
            ds.ReadXml(rutaArchivo);
        }
    }
}