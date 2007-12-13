using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//RENOVAR CON LA NUEVA 
using AForge.Imaging.Filters;

namespace pruebaImplementaciones
{
    public partial class Form1 : Form
    {
        //Almacena la imagen original, la que se carga por defecto
        //o desde un archivo
        Bitmap imagenOriginal;

        public Form1()
        {
            InitializeComponent();
        }

        //Al cargar el formulario se carga la imagen por defecto
        private void Form1_Load(object sender, EventArgs e)
        {
            //Antes se binariza (por si no es realmente binaria)
            //imagenOriginal = Binarizar(Properties.Resources.huella);
            Bitmap bmp = new Bitmap(Properties.Resources.huella);
            imagenOriginal = Binarizar(bmp);

            pictureBox.Image = imagenOriginal;
        }

        //Al hacer click sobre el botón de cargar imagen se abre
        //una ventana para abrir un archivo de imagen
        private void cargar_Click(object sender, EventArgs e)
        {
            //La carpeta por defecto es la de Mis Imágenes
            openFileDialog.InitialDirectory =
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.MyPictures);

            //Se muestra la ventana
            openFileDialog.ShowDialog();

            //Si se cierra la ventana antes de cargar un archivo,
            //leer el nombre de éste provocaría una expceción
            try
            {
                //Se toma la imagen del archivo
                Image archivo = Image.FromFile(openFileDialog.FileName);
                imagenOriginal = new Bitmap(archivo);

                //Antes se binariza (por si no es realmente binaria)
                imagenOriginal = Binarizar(imagenOriginal);

                //Se muestra en el cuadro de imagen
                pictureBox.Image = imagenOriginal;
            }
            catch
            {

            }
        }

        //Al hacer click sobre el botón de reset se carga la
        //imagen original
        private void reset_Click(object sender, EventArgs e)
        {
            pictureBox.Image = imagenOriginal;
        }

        //Al hacer click sobre el botón de ejecutar se muestra
        //la imagen modificada
        private void ejecutar_Click(object sender, EventArgs e)
        {
            Atributos atr = new Atributos();
            atr.colorMinuciaFiable = Color.Blue;
            atr.colorMinuciaPocoFiable = Color.Red;
            atr.colorMinuciaNoFiable = Color.Green;
            atr.radioCirculo = 8;
            atr.radiosL = new int[]{27,45,63,81};
            atr.puntosK = new int[]{10,16,22,28};
            atr.w = 0.5;

            Tratamiento trat = new Tratamiento(imagenOriginal, atr);
            pictureBox.Image = trat.getBitmapFinal();
        }

        //Al CARGAR la imagen desde archivo se pierde algo de 
        //informacion. Es posible que no sea binaria al 100%
        private Bitmap Binarizar(Bitmap imagen)
        {
            Threshold ts = new Threshold();
            ts.ThresholdValue = 128;
            return ts.Apply(imagen);
        }
    }
}