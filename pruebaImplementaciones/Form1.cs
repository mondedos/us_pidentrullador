using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using kernel;

//RENOVAR CON LA NUEVA 
using AForge.Imaging.Filters;

namespace pruebaImplementaciones
{
    public partial class Form1 : Form
    {
        //Almacena la imagen original, la que se carga por defecto
        //o desde un archivo
        Bitmap imagenOriginal;

        Bitmap[] pasos;

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

                //La barra se pone a 0
                trackBar.Value = 0;
                trackBar.Maximum = 0;

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
            //La barra se pone a 0
            trackBar.Value = 0;
            trackBar.Maximum = 0;

            //Se muestra la imagen original
            pictureBox.Image = imagenOriginal;
        }

        //Al CARGAR la imagen desde archivo se pierde algo de 
        //informacion. Es posible que no sea binaria al 100%
        private Bitmap Binarizar(Bitmap imagen)
        {
            Threshold ts = new Threshold();
            ts.ThresholdValue = 128;
            return ts.Apply(imagen);
        }

        //Al hacer click sobre el botón de ejecutar se muestra
        //la imagen modificada
        private void ejecutar_Click(object sender, EventArgs e)
        {
            ejecutar.Enabled = false;

            //El algoritmo se ejecuta en segundo plano
            backgroundWorker.RunWorkerAsync();
        }

        //Funcion que se realiza en segundo plano
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Atributos atr = Atributos.getInstance();
            atr.colorTerminacionFiable = Color.Blue;
            atr.colorTerminacionPocoFiable = Color.BlueViolet;
            
            atr.colorBifurcacionFiable = Color.Green;
            atr.colorBifurcacionPocoFiable = Color.Olive;

            atr.colorMinuciaNoFiable = Color.Red;
            atr.colorLinea = Color.Red;

            atr.maxLongitudBuqueda = 30;

            atr.radioCirculo = 8;

            atr.radiosL = new int[] { 27, 45, 63, 81 };
            atr.puntosK = new int[] { 10, 16, 22, 28 };

            atr.w = 0.5;

            // minimo ha de ser 12
            atr.longitudLinea = 20;

            Tratamiento trat = new Tratamiento(imagenOriginal, atr);

            Bitmap[] vector = trat.getPasos();
            inicializarPasos(vector);
        }

        private void inicializarPasos(Bitmap[] vector)
        {
            //Los pasos incluyen la imagen original
            pasos = new Bitmap[vector.Length + 1];

            pasos[0] = imagenOriginal;

            //El resto de pasos son los del algoritmo
            for (int i = 0; i < vector.Length; i++)
                pasos[i + 1] = vector[i];
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ejecutar.Enabled = true;

            //El tamaño de la barra es el numero de pasos,
            //ademas, al principio es 0
            trackBar.Maximum = pasos.Length-1;
            trackBar.Value = 0;

            //La imagen que se muestra es la original
            pictureBox.Image = pasos[0];
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            pictureBox.Image = pasos[trackBar.Value];
        }

        private void anterior_Click(object sender, EventArgs e)
        {
            if (trackBar.Value > trackBar.Minimum)
                trackBar.Value--;
        }

        private void siguiente_Click(object sender, EventArgs e)
        {
            if (trackBar.Value < trackBar.Maximum)
                trackBar.Value++;

        }
    }
}