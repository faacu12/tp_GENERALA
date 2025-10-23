using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BE; // Asegúrate de agregar la referencia a tu capa BE

namespace GUI
{
    public partial class DadoControl : UserControl
    {
        // Referencia a la instancia de Dado de tu capa BE
        private Dado _dado;

        private readonly string[] _imagenesPath = new string[6]
        {
            "Dados\\dado1.png",
            "Dados\\dado2.png",
            "Dados\\dado3.png",
            "Dados\\dado4.png",
            "Dados\\dado5.png",
            "Dados\\dado6.png"
        };

        // Propiedad para acceder al dado
        public Dado Dado
        {
            get { return _dado; }
            set
            {
                _dado = value;
                ActualizarImagen();
            }
        }

        public DadoControl()
        {
            InitializeComponent();
        }

        private void Dado_Load(object sender, EventArgs e)
        {
            if (_dado == null)
            {
                _dado = new Dado(); // Crear un dado por defecto si no se proporciona uno
            }
            ActualizarImagen();
        }

        // Método para actualizar la imagen según el valor del dado
        public void ActualizarImagen()
        {
            if (_dado == null) return;

            try
            {
                int indice = _dado.Valor - 1; 
                if (indice < 0 || indice >= _imagenesPath.Length) indice = 0;

                string imagePath = Path.Combine(Application.StartupPath, _imagenesPath[indice]);

                if (File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    
                    if (_dado.Retenido)
                    {
                        pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                        pictureBox1.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        pictureBox1.BorderStyle = BorderStyle.None;
                        pictureBox1.BackColor = Color.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen del dado: {ex.Message}");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}