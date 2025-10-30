
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string nombreusuario;

		public string Nombre
		{
			get { return nombreusuario; }
			set { nombreusuario = value; }
		}

		private string contra;

		public string Contraseña
		{
			get { return contra; }
			set { contra = value; }
		}
    }
}
